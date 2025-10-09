---
url: [/post/tao-trang-web-da-ngon-ngu-voi-blazor]
title: "Tạo trang web đa ngôn ngữ với Blazor"
$attribute: PostMetadata(Id = 3, Title = "Tạo trang web đa ngôn ngữ với Blazor", Category = "Blazor", LastModified = "09-10-2025", IsDraft=true)
$layout: BlogContentLayout
---


Nếu bạn muốn website của mình tiếp cận người dùng quốc tế, việc tạo một 
website đa ngôn ngữ (multilingual website) là cần thiết. Trong bài viết này, 
chúng ta sẽ cùng tìm hiểu cách một multilingual website hoạt động, cách lựa 
chọn ngôn ngữ, cách dịch nội dung và cách tải tài nguyên trong Blazor.

# Multilingual website hoạt động thế nào trong Blazor WebAssembly?

Để một website đa ngôn ngữ hoạt động, thường gồm 4 bước cơ bản:
- Xác định ngôn ngữ người dùng: Mỗi quốc gia có thể dùng nhiều ngôn ngữ, 
nên Blazor dựa vào `CultureInfo.DefaultThreadCurrentCulture` và `CultureInfo.DefaultThreadCurrentUICulture` 
để hiển thị nội dung mặc định phù hợp với vùng miền. 
Ví dụ: en-US thì en là tiếng Anh, US là nước Mỹ.
- Tìm file tài nguyên phù hợp: File tài nguyên (resource) chứa văn bản dịch, 	
có thể là `.resx`, `.json`, `.yml`.
- Dùng tham số trong key nếu cần:  Cho phép tạo các chuỗi động với placeholder.
- Hiển thị nội dung đã dịch trên website: Sử dụng các dịch vụ localization để hiển thị nội dung phù hợp.

# Chiến lược lựa chọn ngôn ngữ

Website đa ngôn ngữ cần xác định ngôn ngữ người dùng muốn dùng. Có nhiều cách:
- Sử dụng cookie.
- Sử dụng Accept-Language header
- Sử dụng local storage.
- Sử dụng tham số trong URL.

# Cách dịch nội dung

Có hai cách chính để dịch nội dung trong Blazor:
- Deferred translation: Lưu ngôn ngữ người dùng chọn để dùng cho lần truy cập sau. 
Thường cần refresh trang để áp dụng.
- Instant translation: Khi người dùng chọn ngôn ngữ, nội dung website cập nhật 
ngay lập tức, không cần refresh.

# Cách tải tài nguyên (Resource loading)

Tài nguyên có thể được tải theo nhiều cách:
- Eager loading:vTải tất cả file resource ngay khi trang load nên gây tốn băng 
thông nếu website nhiều file.
- Lazy loading: Chỉ tải file resource cần thiết cho component đang hiển thị. 
Resource được lưu trong cache trong trình duyệt nên nếu đổi ngôn ngữ, không cần tải lại.

Lưu ý: Các phương pháp lazy loading được mô tả chủ yếu áp dụng cho 
Blazor WebAssembly. Trong Blazor Server, tài nguyên thường được tải trên server 
và không có khái niệm "tải resource theo component" theo cách tương tự. 

Trong hướng dẫn này, chúng ta chỉ sử dụng instant translation và lazy loading.

# Lazy loading

Lazily loading resources là cách tiếp cận được khuyến nghị để tải 
resource trong Blazor. Điều này là do nó cung cấp tính linh hoạt cao hơn 
trong việc tải resource và có thể hỗ trợ nhiều loại file resource khác nhau. 

Khi tạo và sử dụng resource trong Blazor, bạn có tùy chọn lưu trữ nội dung 
đã dịch trong file hoặc database. Nếu bạn chọn sử dụng file, các file 
resource nên được lưu trữ trong thư mục wwwroot. Ví dụ:

```text
wwwroot/
└── DemoResources/
    └── Components/
        ├── MyComponent.json
        ├── MyComponent.fr.json
        └── MyComponent.en.json
```

Để đăng ký thư mục chứa các file resource, thêm dòng code sau vào `Program.cs`:
```csharp
builder.Services.AddLocalization(options => options.ResourcesPath = "DemoResources");
```

Tiếp theo bạn cần cài đặt package `Microsoft.Extensions.Localization`.

Nếu cần, tạo cache cho resource để cải thiện hiệu suất:


Tạo culture provider tùy chỉnh:

```csharp
public class DemoLazyCultureProvider
{
    private readonly HttpClient _httpClient;
    private readonly IOptions<LocalizationOptions> _localizationOptions;
    private readonly DemoResourceMemoryStorage _demoResourceMemoryStorage;
    private readonly List<ComponentBase> _subscribedComponents = new();

    public DemoLazyCultureProvider(
        IHttpClientFactory httpClientFactory, 
        DemoResourceMemoryStorage demoResourceMemoryStorage,
        IOptions<LocalizationOptions> localizationOptions)
    {
        _httpClient = httpClientFactory.CreateClient();
        _demoResourceMemoryStorage = demoResourceMemoryStorage;
        _localizationOptions = localizationOptions;
    }

    private async Task<string> LoadCultureAsync(ComponentBase component)
    {
        if (string.IsNullOrEmpty(_localizationOptions.Value.ResourcesPath))
        {
            throw new Exception("ResourcePath not set.");
        }

        string componentName = component.GetType().FullName!;

        if (_demoResourceMemoryStorage.JsonComponentResources.TryGetValue(
            new(componentName, CultureInfo.DefaultThreadCurrentCulture!.Name), 
            out string? resultFromMemory))
        {
            return resultFromMemory;
        }

        var message = await _httpClient.GetAsync(ComposeComponentPath(componentName, CultureInfo.DefaultThreadCurrentCulture!.Name));
        string result;

        if (!message.IsSuccessStatusCode)
        {
            var retryMessage = await _httpClient.GetAsync(ComposeComponentPath(componentName));

            if (!retryMessage.IsSuccessStatusCode)
            {
                throw new Exception($"Cannot find the fallback resource for {componentName}.");
            }
            else
            {
                result = await retryMessage.Content.ReadAsStringAsync();
            }
        }
        else
        {
            result = await message.Content.ReadAsStringAsync();
        }

        _demoResourceMemoryStorage.JsonComponentResources[
            new(componentName, CultureInfo.DefaultThreadCurrentCulture!.Name)
        ] = result;

        return result;
    }

    private string ComposeComponentPath(string componentTypeName, string language = "")
    {
        var nameParts = componentTypeName.Split('.').ToList();
        nameParts.Insert(1, _localizationOptions.Value.ResourcesPath);
        nameParts.RemoveAt(0);
        string componentName = nameParts.Last();
        nameParts[^1] = string.IsNullOrEmpty(language) ? $"{componentName}.json" : $"{componentName}.{language}.json";
        string resourceLocaltion = string.Join("/", nameParts);

        return resourceLocaltion;
    }
    
    public async Task SubscribeLanguageChangeAsync(ComponentBase component)
    {
        _subscribedComponents.Add(component);
        await LoadCultureAsync(component);
    }

    public void UnsubscribeLanguageChange(ComponentBase component) 
        => _subscribedComponents.Remove(component);

    public async Task NotifyLanguageChangeAsync()
    {
        foreach (var component in _subscribedComponents)
        {
            if (component is not null)
            {
                await LoadCultureAsync(component);
                var stateHasChangedMethod = component.GetType()?.GetMethod("StateHasChanged", BindingFlags.Instance | BindingFlags.NonPublic);
                stateHasChangedMethod?.Invoke(component, null);
            }
        }
    }
}
```

Đăng ký culture provider trong file `Program.cs`:
```csharp
builder.Services.AddScoped<DemoLazyCultureProvider>();
```

Khi lazily loading một resource, điều quan trọng là tạo một string localizer 
có thể tìm thấy nội dung đã dịch trong resource dựa trên các key và tham số được 
cung cấp.

Tạo một string localizer:

```csharp
public class DemoStringLocalizer<TComponent> : IStringLocalizer<TComponent> 
    where TComponent : ComponentBase
{
    private readonly DemoResourceMemoryStorage _demoResourceMemoryStorage;

    public LocalizedString this[string name] => FindLocalziedString(name);
    public LocalizedString this[string name, params object[] arguments] 
        => FindLocalziedString(name, arguments);

    public DemoStringLocalizer(
        DemoResourceMemoryStorage demoResourceMemoryStorage)
    {
        _demoResourceMemoryStorage = demoResourceMemoryStorage;
    }

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures) 
        => throw new NotImplementedException("We do not need to implement this method. This method is not support asynchronous anyway.");

    private LocalizedString FindLocalziedString(string name, object[]? arguments = null)
    {
        LocalizedString result = new(name, "", true, "External resource");
        _demoResourceMemoryStorage.JsonComponentResources.TryGetValue(new(typeof(TComponent).FullName!, CultureInfo.DefaultThreadCurrentCulture!.Name), out string? jsonResource);

        if (string.IsNullOrEmpty(jsonResource))
        {
            return result;
        }

        var jObject = JObject.Parse(jsonResource);
        bool success = jObject.TryGetValue(name, out var jToken);

        if (success)
        {
            string value = jToken!.Value<string>()!;

            if (arguments is not null)
            {
                value = string.Format(value, arguments);
            }

            result = new(name, value, false, "External resource");
        }

        return result;
    }
}
```

Đăng ký string localizer trong file `Program.cs`:
```csharp
builder.Services.AddScoped(typeof(IStringLocalizer<>), typeof(DemoStringLocalizer<>));
```

## Lưu trữ với cookies

Nếu một cookie với ngôn ngữ ưa thích tồn tại, website có thể hiển thị nội dung 
bằng ngôn ngữ đó theo mặc định. Tuy nhiên, nếu cookie không tồn tại, Blazor có thể 
sử dụng các chiến lược chọn ngôn ngữ khác như ngôn ngữ trình duyệt hoặc ngôn ngữ 
trong URL. Ngoài ra, website có thể hiển thị nội dung bằng ngôn ngữ fallback. 

Để triển khai chiến lược lưu trữ cookie, hãy làm theo các bước sau:
- Import JavaScript cần thiết để truy cập cookie. 
- Thêm một method trong culture provider của bạn để thiết lập culture 
khởi động dựa trên giá trị cookie:
```csharp
public class BlazorSchoolCultureProvider
{
    private readonly IJSUnmarshalledRuntime _invoker;

    public BlazorSchoolCultureProvider(IJSUnmarshalledRuntime invoker)
    {
        _invoker = invoker;
    }

    public async Task SetStartupLanguageAsync(string fallbackLanguage)
    {
        var jsRuntime = (IJSRuntime)_invoker;
        string cookie = await jsRuntime.InvokeAsync<string>("BlazorSchoolUtil.getCookieValue", CookieRequestCultureProvider.DefaultCookieName);
        var result = CookieRequestCultureProvider.ParseCookieValue(cookie);

        if (result is null)
        {
            var defaultCulture = CultureInfo.GetCultureInfo(fallbackLanguage);
            CultureInfo.DefaultThreadCurrentCulture = defaultCulture;
            CultureInfo.DefaultThreadCurrentUICulture = defaultCulture;
        }
        else
        {
            string storedLanguage = result.Cultures.First().Value;
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(storedLanguage);
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(storedLanguage);
        }
    }
}
```

- Tải các resource và thiết lập ngôn ngữ khởi động trong `Program.cs`:
```csharp
var wasmHost = builder.Build();
var culturesProvider = wasmHost.Services.GetService<BlazorSchoolCultureProvider>();

if (culturesProvider is not null)
{
    await culturesProvider.LoadCulturesAsync("fr", "en");
    await culturesProvider.SetStartupLanguageAsync("fr");
}

await wasmHost.RunAsync();
```
## Triển khai chiến lược local storage

- Thêm một method trong culture provider của bạn để thiết lập culture khởi động 
dựa trên giá trị local storage:
```csharp
public class DemoLazyCultureProvider
{
    // ... các dependency khác
    private readonly DemoLocalStorageAccessor _localStorageAccessor;

    public DemoLazyCultureProvider(..., DemoLocalStorageAccessor localStorageAccessor)
    {
        // ...
        _localStorageAccessor = localStorageAccessor;
    }

    public void SetStartupLanguage(string fallbackLanguage)
    {
        string languageFromLocalStorage = _localStorageAccessor.GetItem("BlazorSchoolInstantTranslation");

        if (string.IsNullOrEmpty(languageFromLocalStorage))
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(fallbackLanguage);
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(fallbackLanguage);
        }
        else
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(languageFromLocalStorage);
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(languageFromLocalStorage);
        }
    }
}
```

## Triển khai chiến lược URL

# Sử dụng instant translation
Sau khi tạo tất cả các class cần thiết và chuẩn bị các resource đã dịch, 
bạn có thể bắt đầu sử dụng instant translation. Khi tạo một component mới, 
điều quan trọng là đăng ký và hủy đăng ký thông báo thay đổi ngôn ngữ:

```markup
@inject IStringLocalizer<ChangeLanguageDemonstrate> Localizer
@inject DemoCultureProvider DemoCultureProvider
@implements IDisposable

<h3>ChangeLanguageDemonstrate</h3>
@Localizer["Hello World {0} {1}", "optional param1", "optional param2"]

@code {
    protected override async Task OnInitializedAsync() 
        => await DemoCultureProvider.SubscribeLanguageChangeAsync(this);
    public void Dispose() 
        => DemoCultureProvider.UnsubscribeLanguageChange(this);
}
```


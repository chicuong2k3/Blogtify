---
url: [/post/doc-co-cau-code-hoi-1-den-4]
title: "Độc cô cầu code - Hồi 1 đến 4"
$attribute: PostMetadata(Id = 1, Title = "Độc cô cầu code - Hồi 1 đến 4", Category = "Truyện coding", LastModified = "23-09-2025")
$layout: BlogContentLayout
---

> *Giữa thế giới lập trình tồn tại vô số "môn phái võ công", mỗi phái mang triết lý và 
tuyệt kỹ riêng. Đây là hành trình của kiếm khách H từ chỗ kiêu ngạo vì biết nhiều chiêu thức, 
đến khi thấu hiểu đạo lý căn cơ của code học.*

# Hồi 1: Kiêu ngọa xuất sơn

Trên đỉnh Võ Đang huyền thoại, nơi được mệnh danh là thánh địa của giới võ lâm,
có một kiếm khách trẻ tuổi tên H. Chàng xuất thân từ môn phái B lừng lẫy trong thiên hạ, 
nơi tụ họp những cao thủ võ lâm tinh thông 
hàng trăm môn tuyệt học từ khắp thiên hạ. 

Suốt bốn năm ròng rèn luyện, H đã thuộc làu vô số bí kíp võ công. 
Từ Thiết chưởng Java nặng đô đến Phi kiếm Python uyển chuyển, 
từ trường thương Spring sắc bén đến song đao Next.js linh hoạt. 
Trong lòng H luôn tự hào rằng mình đã đạt đến cảnh giới chí tôn của võ lâm.

Một ngày nọ, H quyết định xuống núi, cưỡi con chiến mã Ferrari đến thành T phồn hoa, 
nơi bang Lương 5X chiêu mộ anh hùng hào kiệt. 
Trong đại điện nguy nga, H đối diện với lão CTO râu tóc bạc phơ.

"Tiểu tử, ngươi muốn gia nhập bang hội của ta, vậy hãy nói xem ngươi muốn bổng lộc thế nào?" 
lão CTO hỏi với ánh mắt sắc lẹm.

H ngẩng cao đầu: 
*"Tại hạ đã thông thạo tất cả võ công trong thiên hạ, chỉ cần năm mươi lượng vàng và một chiếc Macbook Pro M3 là đủ!"*

Lão CTO khẽ nhếch mép, phất tay ra hiệu. Một kiếm khách áo đen từ trong bóng tối bước ra,
tay cầm cuốn "Framework bí truyền". Chỉ với một chiêu duy nhất, 
kiếm khách áo đen đã khiến H hoảng loạn, những chiêu thức học lỏm từ các tutorial vụn vặt 
lập tức tan thành mây khói. H bại trận thảm hại, toàn thân đầy thương tích, 
phải cuốn gói rời khỏi thành T trong nhục nhã.

Trên đường trở về, dưới ánh trăng mờ ảo bên bờ suối Silicon, H ngồi trầm tư bên đống lửa, 
lòng đầy chán nản: "Ta đã học hàng trăm ngàn chiêu thức, vậy mà vẫn không thể địch nổi 
một chiêu của đối thủ. Có lẽ ta chỉ là kẻ vô dụng, 
mãi mãi không thể với tới cảnh giới của các đại tông sư."

Đúng lúc đó, một lão già râu tóc bạc phơ từ đâu bước tới. 
Ông ngồi xuống bên H, rót một chén trà nóng, chậm rãi bảo: 
"Đồ nhi, con đường code học mênh mông như biển cả, không phải cứ học nhiều chiêu thức là 
có thể thành cao thủ. Con cần phải thấu hiểu căn cơ của từng môn phái, 
sau đó mới có thể dung hợp chúng thành võ công của riêng mình."

# Hồi 2: Thấu hiểu mệnh lệnh chương

Lão già Vô Danh nhấp ngụm trà, bắt đầu giảng giải.
"Trường phái Mệnh lệnh (Imperative Programming) là nền tảng cơ bản nhất của code học. 
Ở trường phái này, người luyện code phải tự mình ra chỉ thị từng bước một, 
kiểm soát mọi chi tiết nhỏ nhất."

Lão lấy một cành cây, vẽ xuống đất một ví dụ: *Tìm những kẻ địch có công lực dưới 10 phần*

```csharp
List<string> enemies = new List<string> { "Độc Đao Tạ", "Bạch Long Mã", "Hắc Mi Kiếm" };
List<int> power = new List<int> { 7, 12, 5 };
List<string> weakEnemies = new List<string>();

for (int i = 0; i < enemies.Count; i++)
{
    if (power[i] < 10)
    {
        weakEnemies.Add(enemies[i]);
    }
}
```

"Con thấy không?" lão Vô Danh bắt đầu giải thích. "Phương pháp này rất trực quan, dễ hiểu. 
Mỗi bước đều rõ ràng: duyệt qua từng kẻ địch, kiểm tra công lực, và quyết định hành động."

"Nhưng phương pháp này có nhược điểm lớn." Lão nghiêm mặt nói. 
"Khi số lượng kẻ địch tăng lên, hoặc yêu cầu phức tạp hơn, code sẽ trở nên rối rắm 
như mạng nhện. Mỗi lần sửa đổi đều có nguy cơ phá vỡ cả hệ thống."

H gật đầu: "Vâng, con đã biết cách viết này từ lúc nhập môn rồi, 
nhưng thật không ngờ nó còn có cả tên gọi riêng."


# Hồi 3: Hướng đối tượng chân truyền

Lão Vô Danh mỉm cười: "Để khắc phục nhược điểm của trường phái Mệnh lệnh, các đại tông sư 
đã sáng tạo ra trường phái Hướng đối tượng (Object-Oriented Programming). Ở trường phái này, mọi sự vật đều được 
xem như các đối tượng độc lập, tự quản lý trạng thái và hành vi của chính mình."

Lão đưa ra một ví dụ khác:

```csharp
class Enemy
{
    public string Name { get; }
    public int Power { get; }
    public bool IsWeak => Power < 10;

    public Enemy(string name, int power)
    {
        Name = name;
        Power = power;
    }

    public void EvaluateThreat()
    {
        if (IsWeak)
            Console.WriteLine($"{Name} có công lực yếu, có thể tấn công!");
        else
            Console.WriteLine($"{Name} quá mạnh, cần thận trọng!");
    }
}
```


```csharp
List<Enemy> enemies = new List<Enemy>
{
    new Enemy("Độc Đao Tạ", 7),
    new Enemy("Bạch Long Mã", 12),
    new Enemy("Hắc Mi Kiếm", 5)
};

foreach (var enemy in enemies)
{
    enemy.EvaluateThreat();
}
```

"Thấy không?" lão Vô Danh giải thích. 
"Mỗi Enemy là một đối tượng độc lập, tự quản lý thông tin và hành vi của nó. 
Đây chính là nguyên lý Đóng gói (Encapsulation) trong OOP."

"Khi thêm kẻ địch mới, ta chỉ cần tạo đối tượng mới mà không cần sửa code cũ. 
Khi yêu cầu thay đổi, ta chỉ cần sửa lớp Enemy mà không ảnh hưởng đến các phần khác."

Tuy nhiên, lão cũng cảnh báo: "OOP vẫn có điểm yếu. Khi các đối tượng liên tục thay đổi 
trạng thái lẫn nhau, sẽ khó theo dõi và kiểm soát, có thể gây ra side effect 
(hiện tượng một phương thức vô tình thay đổi trạng thái bên ngoài phạm vi của nó, chẳng hạn như biến toàn cục). 
Đặc biệt trong các trận chiến đa luồng, tình trạng race condition có thể xảy ra."

# Hồi 4: Càn khôn đại na di của lập trình

Lão Vô Danh nhấp thêm ngụm trà, ánh mắt trở nên sâu thẳm.

"Để giải quyết tận gốc vấn đề, các đại tông sư đã sáng tạo ra trường phái Hàm 
(Functional Programming). Trường phái này đề cao sự thuần khiết và bất biến.
Đây chính là Càn Khôn Đại Na Di trong thế giới lập trình, 
nguyên tắc của trường phái này là biến đổi dữ liệu mà không làm thay đổi 
trạng thái gốc, mọi thứ đều vận hành theo quy luật bất di bất dịch của sự thuần khiết và bất biến.

H reo lên: "Tiền bối! Có phải những hàm `Select`, `Where` mà đệ tử thường dùng chính là..."

"Đúng vậy!" lão Vô Danh gật đầu. "Con đã vô tình chạm đến cảnh giới của Functional Programming 
mà không hay biết."

Lão đưa ra ví dụ:

```csharp
List<Enemy> enemies = new List<Enemy>
{
    new Enemy("Độc Đao Tạ", 7),
    new Enemy("Bạch Long Mã", 12),
    new Enemy("Hắc Mi Kiếm", 5)
};

var weakEnemies = enemies
    .Where(enemy => enemy.Power < 10)
    .Select(enemy => enemy.Name)
    .ToList();

weakEnemies.ForEach(name => 
    Console.WriteLine($"{name} có công lực yếu, có thể tấn công!"));
```

"Ở đây, `Where` và `Select` là các hàm thuần túy. 
Chúng nhận đầu vào là danh sách `enemies` và trả về danh sách mới `weakEnemies`, 
không hề thay đổi danh sách gốc. Đây chính là sức mạnh của Functional Programming. 
Ta sẽ chỉ cho con một số nguyên tắc cốt lõi trong FP."

## Nguyên tắc 1: Hàm như giá trị (First-class functions)

"Trong Functional Programming, hàm được xem như bất kỳ giá trị nào khác: 
có thể truyền vào hàm khác, trả về từ hàm, hoặc gán vào biến."

```csharp
// Gán hàm vào biến
Func<int, int> square = x => x * x;

// Truyền hàm như tham số
var numbers = Enumerable.Range(1, 5);
var squares = numbers.Select(square);

Console.WriteLine($"Bình phương: {string.Join(", ", squares)}");
// Output: Bình phương: 1, 4, 9, 16, 25
```

## Nguyên tắc 2: Tính bất biến (Immutability)

"Dữ liệu không bao giờ thay đổi sau khi được tạo. 
Thay vì sửa đổi dữ liệu cũ, ta phải luôn tạo dữ liệu mới."

```csharp
var original = new List<int> { 1, 2, 3 };

// Thay vì sửa trực tiếp: original.Add(4);
var updated = original.Append(4); // Tạo danh sách mới

Console.WriteLine("Original: " + string.Join(", ", original));
Console.WriteLine("Updated: " + string.Join(", ", updated));

// Output
// Original: 1, 2, 3
// Updated: 1, 2, 3, 4
```

## Nguyên tắc 3: Hàm bậc cao (Higher-Order Functions)

"Hàm bậc cao là hàm nhận hàm khác làm tham số hoặc trả về hàm. 
Đây là vũ khí lợi hại nhất của Functional Programming."

Ví dụ tự triển khai `Where`:

```csharp
public static IEnumerable<T> Where<T>(this IEnumerable<T> source, Func<T, bool> predicate)
{
    if (source == null) throw new ArgumentNullException(nameof(source));
    if (predicate == null) throw new ArgumentNullException(nameof(predicate));

    foreach (T item in source)
    {
        if (predicate(item))
        {
            yield return item;
        }
    }
}
```


"Sau đây là một số loại Higher-Order Functions"

**Adapter Functions**

"Được sử dụng khi ta cần thay đổi cách gọi hàm mà không muốn sửa hàm gốc."

```csharp
// Adapter đảo thứ tự tham số
public static class FunctionAdapters
{
    public static Func<T2, T1, R> SwapArgs<T1, T2, R>(Func<T1, T2, R> func)
    {
        return (t2, t1) => func(t1, t2);
    }
}

// Hàm chia hai số: x / y
Func<int, double, double> divide = (x, y) => x / y; 
// Hàm chia hai số: y / x
Func<double, int, double> divideBy = FunctionAdapters.SwapArgs(divide);
```

**Function Factories**

"Được dùng để tạo ra các hàm được tùy chỉnh sẵn theo ngữ cảnh."

```csharp
// Factory tạo hàm kiểm tra độ dài chuỗi
Func<int, int, Func<string, bool>> CreateLengthValidator = 
    (min, max) => text => text != null && text.Length >= min && text.Length <= max;

// Tạo các hàm kiểm tra cụ thể
var isShortName = CreateLengthValidator(2, 10);
var isLongPassword = CreateLengthValidator(8, 20);

Console.WriteLine($"'Tom' là tên ngắn? {isShortName("Tom")}");
Console.WriteLine($"'123' là mật khẩu dài? {isLongPassword("123")}");
```

"Mỗi trường phái đều có ưu nhược điểm riêng." lão giải thích. 
"Người lập trình giỏi không cứng nhắc theo một trường phái, mà biết kết hợp linh hoạt chúng với nhau."


Lão Vô Danh đứng dậy, phủi bụi trên áo.

"Đồ nhi, trong thế giới lập trình không có trường phái nào là hoàn hảo tuyệt đối. 
Người cao thủ thực thụ là người biết vận dụng linh hoạt các trường phái khác nhau, 
tùy theo tình huống mà xuất chiêu."

Nói rồi, lão Vô Danh quay lưng bước vào màn sương.
H nhìn theo bóng lão với ánh mắt kiên định. Chàng đã hiểu rằng thực lực không đến từ số lượng chiêu thức, mà đến từ độ sâu của sự thấu hiểu.
Từ đó, H không còn mải mê săn lùng các framework mới, mà quyết tâm tập trung rèn luyện nền tảng căn cơ, 
chờ ngày phục thù. 

<center>To be continue...</center>
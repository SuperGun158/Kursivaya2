using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;
using System.Security.Cryptography;

HttpClient client = new HttpClient();
string HashPassword(string password) {
    using (var algorithm = SHA256.Create()) {
        var bytes_hash = algorithm.ComputeHash(Encoding.Unicode.GetBytes(password));
        return Encoding.Unicode.GetString(bytes_hash);
    }
}
Token? Login(string? _username, string? _password){
    if (_username == null || _password == null || _username.Length == 0 || _password.Length == 0){
        return null;
    }
    string request = "/login";
    var json_data = new {
        username = _username,
        password = HashPassword(_password)
    };
    string jsonBody = JsonSerializer.Serialize(json_data);
    var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
    var response = client.PostAsync(request, content).Result;
    if (response.IsSuccessStatusCode){
        Story("User " + _username + " log in!");
        return response.Content.ReadFromJsonAsync<Token>().Result;
    }
    else{
        return null;
    }
}
Token? Change(string? _username, string? _password){
    if (_username == null || _password == null || _username.Length == 0 || _password.Length == 0){
        return null;
    }
    string request = "/replace";
    var json_data = new {
        username = _username,
        password = HashPassword(_password)
    };
    string jsonBody = JsonSerializer.Serialize(json_data);
    var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
    var response = client.PostAsync(request, content).Result;
    if (response.IsSuccessStatusCode){
        Story("User " + _username + " change password!");
        return response.Content.ReadFromJsonAsync<Token>().Result;
    }
    else{
        return null;
    }
}
bool Register(string? _username, string? _password){
    if (_username == null || _password == null || _username.Length == 0 || _password.Length == 0){
        return false;
    }
    else{
        string request = "/reg";
        var json_data = new {
            username = _username,
            password = HashPassword(_password)
        };
        string jsonBody = JsonSerializer.Serialize(json_data);
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
        var response = client.PostAsync(request, content).Result;
        if (response.IsSuccessStatusCode){
            Story("User " + _username + " registered!");
            return true;
        }
        else{
            return false;
        }
    }
}
bool DeleteAcc(string? _username, string? _password){
    if (_username == null || _password == null || _username.Length == 0 || _password.Length == 0){
        Console.WriteLine("Lost!");
        return false;
    }
    else{
        string request = "/delete";
        var json_data = new {
            username = _username,
            password = HashPassword(_password)
        };
        string jsonBody = JsonSerializer.Serialize(json_data);
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
        var response = client.PostAsync(request, content).Result;
        if (response.IsSuccessStatusCode){
            Story("User " + _username + " deleted!");
            return true;
        }
        else{
            return false;
        }
    }
}
string? Reader(int inx){
    string? z = File.ReadLines("/home/gay/ConsoleAppSuperGay/ConsoleAppSuperGay/story.txt").ElementAtOrDefault(inx);
    return z;
}
void Story(string step){
    string path = "/home/gay/ConsoleAppSuperGay/ConsoleAppSuperGay/story.txt";
    List<string> z = new List<string>(File.ReadAllLines(path));
    z.Add(step + " " + DateTime.Now);
    File.WriteAllLines(path,z);
}
void Replace(string username, string list, string library){
    string path = "/home/gay/ConsoleAppSuperGay/ConsoleAppSuperGay/story.txt";
    string[] z = File.ReadAllLines(path);
    z[0] = username;
    z[1] = list;
    z[2] = library;
    File.WriteAllLines(path,z);
}
void Clear(string username){
    string path = "/home/gay/ConsoleAppSuperGay/ConsoleAppSuperGay/story.txt";
    string[] z = [username];
    File.WriteAllLines(path,z);
}
string JSON(string _zov, string _list, string _library, string _inx1, string _inx2, string url){
    string request = "/" + url;
    var json_data = new {
        zov = _zov,
        list = _list,
        library = _library,
        inx1 = _inx1,
        inx2 = _inx2
    };
    string jsonBody = JsonSerializer.Serialize(json_data);
    var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
    var response = client.PostAsync(request, content).Result;
    if (response.IsSuccessStatusCode){
        return response.Content.ReadAsStringAsync().Result;
    }
    else{
        Console.WriteLine("ZZZVVVOOO");
        return null;
    }
}
try
{
    const string DEFAULT_SERVER_URL = "http://localhost:5062/";
    string server_url = DEFAULT_SERVER_URL;
    client.BaseAddress = new Uri(server_url);
    while (true){
        string? username;
        string? password;
        string list = "";
        string library = "";
        Console.WriteLine("Register (1) or logining (2)?");
        string prov = Console.ReadLine();
        Console.Clear();
        if (prov == "1"){
            Console.WriteLine("Enterlogin and password:");
            username = Console.ReadLine();
            password = Console.ReadLine();
            if (!Register(username, password)){
                Console.Clear();
                Console.WriteLine("Error!");
                continue;
            }
        }
        Console.WriteLine("Enter your login and password:");
        username = Console.ReadLine();
        password = Console.ReadLine();
        Token? token = Login(username, password);
        if (token == null){
            Console.Clear();
            Console.WriteLine("Login or password is false!");
            continue;
        }
        Console.Clear();
        DateTime ZSWOZ = DateTime.Now.AddHours(2);
        Console.WriteLine("Start work, delete account, change password or clear story and save? (start, delete, change or clear)");
        string ZaNashih = Console.ReadLine();
        if (ZaNashih == "delete"){
            Console.Clear();
            Console.WriteLine("Repeat your login and password!");
            if (username == Console.ReadLine()){
                if (password == Console.ReadLine()){
                    Console.Clear();
                    DeleteAcc(username, password);
                    continue;
                }
            }
            Console.Clear();
            Console.WriteLine("Wrong!");
            continue;
        }
        else if (ZaNashih == "change"){
            Console.Clear();
            Console.WriteLine("Repeat your login and password!");
            if (username == Console.ReadLine()){
                if (password == Console.ReadLine()){
                    Console.Clear();
                    Console.WriteLine("Enter new password:");
                    token = Change(username, Console.ReadLine());
                    continue;
                }
            }
            Console.Clear();
            Console.WriteLine("Wrong!");
            continue;
        }
        else if (ZaNashih == "clear"){
            Console.Clear();
            Console.WriteLine("Sure? (Yes or No)");
            if (Console.ReadLine() == "Yes"){
                Clear(username);
            }
        }
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value.access_token);
        while (true){
            bool zprov = true;
            int patriot = 0;
            int t = 0;
            if (Reader(0) == username && Reader(1) != null){
                Console.Clear();
                Console.WriteLine("Start from save? (Yes or No)");
                if (Console.ReadLine() == "Yes"){
                    if (ZSWOZ < DateTime.Now){
                        break;
                    }
                    list = Reader(1);
                    library = Reader(2);
                    zprov = false;
                    Story("User " + username + " start from save!");
                }
            }
            if (zprov){
                Console.Clear();
                Console.WriteLine("Random list or not? (Yes or No)");
                if (Console.ReadLine() == "Yes"){
                    if (ZSWOZ < DateTime.Now) break;
                    Console.Clear();
                    Console.WriteLine("List size:");
                    string? boss = Console.ReadLine();
                    if (ZSWOZ < DateTime.Now) break;
                    if (boss != null){
                        while (!int.TryParse(boss, out t) || Convert.ToInt32(boss) < 0){
                            Console.Clear();
                            Console.WriteLine("Try again:");
                            boss = Console.ReadLine();
                            if (ZSWOZ < DateTime.Now) break;
                        }
                    }
                    Console.Clear();
                    Console.WriteLine("Your library thouth ; :");
                    library = Console.ReadLine();
                    if (ZSWOZ < DateTime.Now) break;
                    if (library == "") library = "Red;Blue;White";
                    Story("User " + username + " create library: " + library);
                    if (boss == null) list = JSON("", "", library, "10", "0", "rand");
                    else list = JSON("", "", library, boss.ToString(), "0", "rand");
                    if (ZSWOZ < DateTime.Now) break;
                    Console.WriteLine(list);
                    Story("User " + username + " create list: " + list);
                }
                else{
                    while (true){
                        Console.Clear();
                        Console.WriteLine("Your library thouth \";\":");
                        library = Console.ReadLine();
                        if (ZSWOZ < DateTime.Now) break;
                        if (library != ""){
                            break;
                        }
                    }
                    Story("User " + username + " create library: " + library);
                    while (true){
                        Console.Clear();
                        Console.WriteLine("Your list thouth \";\":");
                        list = Console.ReadLine();
                        if (ZSWOZ < DateTime.Now) break;
                        if (list != ""){
                            bool Man = true;
                            foreach (string Aboy in list.Split(";")){
                                Man = true;
                                foreach (string Zboy in library.Split(";")) {
                                    if (Zboy == Aboy) Man = false;
                                }
                                if (Man) break;
                            }
                            if (!Man) break;
                        }
                    }
                    Story("User " + username + " create list: " + list);
                }
            }
            while (true) {
                Console.Clear();
                Console.WriteLine("Add, sort or end work? (1 or 2 or close)");
                string boy = Console.ReadLine();
                if (ZSWOZ < DateTime.Now) break;
                if (boy == "1"){
                    Console.Clear();
                    Console.WriteLine("Index of start: (<0 add in start, >Length of list add in end)");
                    string? boss3 = Console.ReadLine();
                    if (ZSWOZ < DateTime.Now) break;
                    while (!int.TryParse(boss3, out t)){
                        Console.Clear();
                        Console.WriteLine("Try again!");
                        boss3 = Console.ReadLine();
                    }
                    Console.Clear();
                    Console.WriteLine("List of odject to add thouth \";\":");
                    string zov = "";
                    do{
                        zov = Console.ReadLine();
                        if (ZSWOZ < DateTime.Now) break;
                    }
                    while (zov == "");
                    if(Convert.ToInt32(boss3) < 0) list = JSON(zov, list, library, "-1", "", "add");
                    else list = JSON(zov, list, library, boss3.ToString(), "", "add");
                    Story("User " + username + " add " + zov + " and create list: " + list);
                }
                else if (boy == "close"){
                    break;
                }
                else{
                    Console.Clear();
                    Console.WriteLine("Index of start and end thouth space(default first and last):");
                    string[]? boys = Console.ReadLine().Split(" ");
                    if (ZSWOZ < DateTime.Now) break;
                    if (boys.Length == 2) list = JSON("", list, library, boys[0], boys[1], "sort");
                    else list = JSON("", list, library, "0", (list.Split(";").Length - 1).ToString(), "sort");
                    Console.WriteLine(list);
                    Story("User " + username + " sort list and create: " + list);
                }
            }
            Console.Clear();
            Console.WriteLine("It's all for this account? (Yes or No)");
            if (ZSWOZ < DateTime.Now){
                Story("JWT Token of user " + username + " expired.");
                break;
            } 
            if (Console.ReadLine() == "Yes"){
                Story("User " + username + " create came out.");
                Replace(username, list, library);
                break;
            }
        }
        Console.Clear();
        if (ZSWOZ < DateTime.Now){
            Console.WriteLine("You have exceed your session time. Start again? (Yes or No)");
        }
        else{
            Console.WriteLine("It's exactly all? (Yes or No)");
        }
        if (Console.ReadLine() == "Yes"){
            break;
        }
    }
}
catch (Exception exp)
{
    Console.WriteLine(exp.ToString);
}
public struct Token{
    public required string access_token { get; set; }
    public string username { get; set; }
}
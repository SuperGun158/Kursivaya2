using System.Data.Common;
using System.IdentityModel.Tokens.Jwt;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthorization();
bool CustomLifeTimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters){
    if (expires == null) return false;
    return DateTime.UtcNow < expires;
}
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = true,
        ValidIssuer = AuthOptions.ISSUER,
        ValidateAudience = true,
        ValidAudience = AuthOptions.AUDIENCE,
        ValidateLifetime = true,
        LifetimeValidator = CustomLifeTimeValidator,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = AuthOptions.GetKey()
    };
});
var app = builder.Build();
app.UseStaticFiles();
app.UseDefaultFiles();
app.UseAuthentication();
app.UseAuthorization();
ServerUser lg = new ServerUser();
DBManager db = new DBManager();
Backend sort = new Backend();
const string DB_PATH = "/home/gay/ConsoleAppSuperGay/Zov/database/users.db";
app.MapPost("/reg", ([FromBody] Login log) => {
    if (db.AddUser(log.username, log.password)) return Results.Ok("Man " + log.username + " zaregestrirovalsa!!");
    else return Results.Problem("Nepoluchilos :(");
    
});
app.MapPost("/delete", ([FromBody] Login log) => db.Delete(log.username));
app.MapPost("/replace", ([FromBody] Login log) => db.Replace(log.username, log.password, db));
app.MapPost("/login", ([FromBody] Login log) => lg.LogIn(log.username, log.password, db));
if (!db.ConnectToDB(DB_PATH)){
    Console.WriteLine(DB_PATH + " - neverniy way!");
    return;
}
app.MapPost("/sort", [Authorize] ([FromBody] Borders bs) => sort.Sort(bs.list,bs.library,bs.inx1,bs.inx2));
app.MapPost("/add", [Authorize] ([FromBody] Borders bs) => sort.Add(bs.zov, bs.inx1, bs.list));
app.MapPost("/rand", [Authorize] ([FromBody] Borders bs) => sort.Random(bs.library, bs.inx1));
app.Run();
db.Disconnect();
struct Borders {
    public string zov { get; set; }
    public string list { get; set; }
    public string library { get; set; }
    public string inx1 { get; set; }
    public string inx2 { get; set; }
}
struct Login{
    public string username { get; set; }
    public string password { get; set; }
}
public class AuthOptions {
    public const string ISSUER = "WebAppTest";
    public const string AUDIENCE = "WebAppTestAudience";
    public static SymmetricSecurityKey GetKey() {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            "WebAppTestPasswordWebAppTestPasswordWebAppTestPasswordWebAppTestPassword"));
    }
}
public class ServerUser{
    public IResult LogIn(string login, string password, DBManager db){
        if (db.CheckUser(login, password)){
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                expires: DateTime.UtcNow.AddMinutes(120),
                signingCredentials: new SigningCredentials(
                    AuthOptions.GetKey(), SecurityAlgorithms.HmacSha256
                )
            );
            var encodedToken = new JwtSecurityTokenHandler().WriteToken(jwt);
            var response = new {
                access_token = encodedToken,
                username = login
            };
            return Results.Ok(response);
        }
        return Results.Unauthorized();
    }
}
public class DBManager{
    private SqliteConnection? connection = null;
    public bool ConnectToDB(string path){
        try
        {
            connection = new SqliteConnection("Data Source=" + path);
            if (null == connection) return false;
            connection.Open();
            if (connection.State != System.Data.ConnectionState.Open){
                Console.WriteLine("nePovezlo");
                return false;
            }
            Console.WriteLine("Povezlo");
            return true;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return false;
        }
    }
    public void Disconnect(){
        try
        {
            if (null == connection || connection.State != System.Data.ConnectionState.Open) return;
            connection.Close();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return;
        }
    }
    public bool AddUser(string login, string password){
        try
        {
            if (null == connection || connection.State != System.Data.ConnectionState.Open) return false;
            string REQUEST = "INSERT INTO users (login, password) VALUES ('" + login + "', '" + password + "')";
            var command = new SqliteCommand(REQUEST, connection);
            int result = command.ExecuteNonQuery();
            if (result == 1) return true;
            else return false;
        }   
        catch(Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return false;
        }
    }
    public bool Delete(string login){
        try
        {
            if (null == connection || connection.State != System.Data.ConnectionState.Open) return false;
            string REQUEST = "DELETE FROM users WHERE login = '" + login + "'";
            var command = new SqliteCommand(REQUEST, connection);
            int result = command.ExecuteNonQuery();
            if (result == 1) return true;
            else return false;
        }   
        catch(Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return false;
        }
    }
    public IResult Replace(string login, string password, DBManager db){
        try
        {
            if (null == connection || connection.State != System.Data.ConnectionState.Open) return Results.Unauthorized();
            string REQUEST = "REPLACE INTO users (login, password) VALUES ('" + login + "', '" + password + "')";
            var command = new SqliteCommand(REQUEST, connection);
            int result = command.ExecuteNonQuery();
            ServerUser zov = new ServerUser();
            return zov.LogIn(login, password, db);
        }   
        catch(Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return Results.Unauthorized();
        }
    }
    public bool CheckUser(string login, string password){
        try
        {
            if (null == connection || connection.State != System.Data.ConnectionState.Open) return false;
            string REQUEST = "SELECT login, password FROM users WHERE login = '" + login + "' AND password = '" + password + "'";
            var command = new SqliteCommand(REQUEST, connection);
            var reader = command.ExecuteReader();
            if (reader.HasRows) return true;
            else return false;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return false;
        }
    }
}

public class Backend
{
    public string Sort(string _s, string _l, string _inx1, string _inx2)
    {
        string[] s = _s.Split(";");
        string[] l = _l.Split(";");
        int inx1 = Convert.ToInt32(_inx1);
        int inx2 = Convert.ToInt32(_inx2);
        int t = inx1;
        while(t < inx2)
        {
            if (Array.IndexOf(l,s[t]) > Array.IndexOf(l,s[t+1]))
            {
                string z = s[t];
                s[t] = s[t+1];
                s[t+1] = z;
                if (t == inx1)
                {
                    t++;
                }
                else
                {
                    t--;
                }
            }
            else t++;
        }
        string n = s[0];
        foreach (string g in s[1..]) n += ";" + g;
        return n;
    }
    public string Add(string zov, string _inx, string _s)
    {
        string[] s = _s.Split(";");
        int inx = Convert.ToInt32(_inx);
        string rus = "";
        if (inx < 0){
            return zov + ";" + _s;
        }
        if (inx >= s.Length){
            return _s + ";" + zov;
        }
        for (int i = 0; i < s.Length; i++)
        {
            rus += s[i];
            if (i != s.Length -1) rus += ";";
            if (i == inx)
            {
                rus += zov;
                if (i != s.Length -1) rus += ";";
            }
            Console.WriteLine(rus);
            Console.WriteLine(s.Length);
        }
        return rus;
    }
    public string Random(string _library, string _inx){
        string[] library = _library.Split(";");
        int inx = Convert.ToInt32(_inx);
        Random rand = new Random();
        string list = "";
        for (int i = 0; i < inx; i++){
            list += library[rand.Next(0, library.Length)];
            if (i != inx-1) list += ";";
        }
        return list;
    }
}
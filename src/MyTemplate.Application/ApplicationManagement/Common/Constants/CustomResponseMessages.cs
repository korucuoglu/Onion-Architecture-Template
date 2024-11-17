namespace MyTemplate.Application.ApplicationManagement.Common.Constants;

public class CustomResponseMessages
{
    public const string UnAuthenticated = "Sisteme giriş yapılmamış.";
    public const string UnAuthorized = "Yetkisiz giriş tespit edildi";
    public const string UserNotFound = "Kullanıcı bulunamadı. Şifre veya kullanıcı adı yanlış.";
    public const string EmailNotConfirmed = "Email adresi henüz onaylanmamış. ";
    public const string InvalidUrl = "Bu link geçersizdir.";
    public const string InvalidToken = "Bu token geçersiz veya süresi dolmuştur.";
    public const string EmailConfirmed = "Mail adresi başarıyla onaylandı.";
    public const string EmailSended = "Gerekli bilgiler mail adresine gönderildi";
    public const string PasswordChanged = "Parola başarıyla güncellendi.";
    public const string UserInfoChanged = "Kullanıcı bilgileri başarıyla güncellendi.";
    public const string RePasswordError = "Girilen parolalar birbirlerinden farklı.";

}
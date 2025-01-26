using Microsoft.AspNetCore.Authorization;
using MyTemplate.Application.AuthManagement.Password.ChangePassword;
using Swashbuckle.AspNetCore.Annotations;

namespace MyTemplate.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[SwaggerTag("Kimlik doğrulama ve yetkilendirme işlemleri")]
public class AuthController : BaseApiController
{
    [HttpGet("validate/mail/{Token}")]
    [SwaggerOperation(Summary = "E-posta doğrulama", Description = "Kullanıcının e-posta adresini doğrular")]
    [SwaggerResponse(200, "E-posta başarıyla doğrulandı")]
    [SwaggerResponse(400, "Geçersiz token")]
    public async Task<IActionResult> ValidateMailAsync([FromRoute] Application.AuthManagement.ValidateMail.Command command)
     => Result(await Mediator.Send(command));
    
    [HttpGet("validate/reset-password-token/{Token}")]
    [SwaggerOperation(Summary = "Şifre sıfırlama tokenı doğrulama", Description = "Şifre sıfırlama tokenının geçerliliğini kontrol eder")]
    [SwaggerResponse(200, "Token geçerli")]
    [SwaggerResponse(400, "Geçersiz token")]
    public async Task<IActionResult> ValidateTokenAsync([FromRoute] Application.AuthManagement.Password.ValidateResetPasswordToken.Command command)
        => Result(await Mediator.Send(command));

    [HttpPost("register")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Kullanıcı kaydı", Description = "Yeni kullanıcı kaydı oluşturur")]
    [SwaggerResponse(200, "Kullanıcı başarıyla kaydedildi")]
    [SwaggerResponse(400, "Geçersiz kayıt bilgileri")]
    public async Task<IActionResult> RegisterAsync([FromBody] Application.AuthManagement.Register.Command command)
      => Result(await Mediator.Send(command));

    [HttpPost("login")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Kullanıcı girişi", Description = "Kullanıcı girişi yapar ve JWT token döner")]
    [SwaggerResponse(200, "Giriş başarılı, JWT token döndürüldü")]
    [SwaggerResponse(401, "Geçersiz kullanıcı adı veya şifre")]
    public async Task<IActionResult> LoginAsync([FromBody] Application.AuthManagement.Login.Command command)
     => Result(await Mediator.Send(command));

    [HttpPost("update")]
    [Authorize]
    [SwaggerOperation(Summary = "Kullanıcı bilgilerini güncelleme", Description = "Mevcut kullanıcının bilgilerini günceller")]
    [SwaggerResponse(200, "Kullanıcı bilgileri başarıyla güncellendi")]
    [SwaggerResponse(401, "Yetkisiz erişim")]
    public async Task<IActionResult> UpdateAsync([FromBody] Application.AuthManagement.Update.Command command)
   => Result(await Mediator.Send(command));

    [HttpPost("change-password")]
    [Authorize]
    [SwaggerOperation(Summary = "Şifre değiştirme", Description = "Kullanıcının şifresini değiştirir")]
    [SwaggerResponse(200, "Şifre başarıyla değiştirildi")]
    [SwaggerResponse(401, "Yetkisiz erişim")]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] Command command)
  => Result(await Mediator.Send(command));
    
    [HttpPost("reset-password")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Şifre sıfırlama talebi", Description = "Kullanıcının şifre sıfırlama talebini başlatır")]
    [SwaggerResponse(200, "Şifre sıfırlama e-postası gönderildi")]
    [SwaggerResponse(400, "Geçersiz e-posta adresi")]
    public async Task<IActionResult> ResetPasswordAsync([FromBody] Application.AuthManagement.Password.ResetPassword.Command command)
        => Result(await Mediator.Send(command));
    
    [HttpPost("reset-password/confirm")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Şifre sıfırlama onayı", Description = "Şifre sıfırlama işlemini tamamlar")]
    [SwaggerResponse(200, "Şifre başarıyla sıfırlandı")]
    [SwaggerResponse(400, "Geçersiz token veya şifre")]
    public async Task<IActionResult> ResetPasswordConfirmAsync([FromBody] Application.AuthManagement.Password.ResetPasswordConfirm.Command command)
        => Result(await Mediator.Send(command));
}
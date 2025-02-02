# Onion Architecture Template

Bu proje, Clean Architecture (Temiz Mimari) prensiplerini temel alan ve Onion Architecture (Soğan Mimarisi) pattern'i kullanılarak geliştirilmiş bir .NET Core template projesidir.

## 🎯 Proje Hakkında

Bu template, kurumsal düzeyde uygulamalar geliştirmek için gerekli olan temel yapıyı ve en iyi pratikleri içermektedir. Onion Architecture'ın katmanlı yapısı sayesinde, uygulamanızın bakımını kolaylaştırır ve test edilebilirliğini artırır.

## 🏗️ Mimari Yapı

Proje aşağıdaki katmanlardan oluşmaktadır:

### Core Katmanları
- **MyTemplate.Domain**: Uygulamanın çekirdek iş mantığını, entities, value objects ve domain events gibi temel yapıları içerir.
- **MyTemplate.Application**: Use case'leri, iş kurallarını ve domain layer'ın dış dünya ile olan iletişimini yönetir.

### Dış Katmanlar
- **MyTemplate.Infrastructure**: Veritabanı işlemleri, harici servis entegrasyonları, dosya sistemi işlemleri gibi dış kaynak etkileşimlerini yönetir.
- **MyTemplate.API**: REST API endpoints'lerini ve HTTP ile ilgili tüm işlemleri içerir.
- **MyTemplate.WorkerService**: Arka plan işlemleri ve zamanlanmış görevler için worker service implementasyonunu içerir.

## 🚀 Kurulum

### Template'i Yükleme

Template'i .NET CLI aracılığıyla doğrudan GitHub'dan yükleyebilirsiniz. İstediğiniz .NET sürümüne göre aşağıdaki komutlardan uygun olanı kullanabilirsiniz:

#### .NET 9.0 için (varsayılan):
```bash
dotnet new install github.com/korucuoglu/Onion-Architecture-Template
```

#### .NET 7.0 için:
```bash
dotnet new install github.com/korucuoglu/Onion-Architecture-Template#net7.0
```

> **Not**: Farklı .NET sürümleri için branch'ler:
> - `main` branch (varsayılan): .NET 9.0
> - `net7.0` branch: .NET 7.0
> 
> Her branch, ilgili .NET sürümüne özgü paket versiyonlarını ve yapılandırmaları içerir. İhtiyacınıza göre uygun branch'i seçebilirsiniz.

### Yeni Proje Oluşturma

Template yüklendikten sonra, aşağıdaki komut ile yeni bir proje oluşturabilirsiniz:

```bash
dotnet new korucuoglu-template -o "ProjeminAdi"
```

Bu komut, belirttiğiniz isimde yeni bir dizin oluşturacak ve seçtiğiniz .NET sürümüne uygun Onion Architecture yapısında bir proje oluşturacaktır.

### Örnek Kullanım Senaryoları

1. .NET 9.0 projesi oluşturmak için (varsayılan):
```bash
# Varsayılan template'i yükleyin (.NET 9.0)
dotnet new install github.com/korucuoglu/Onion-Architecture-Template

# Yeni proje oluşturun
dotnet new korucuoglu-template -o "MyNet9Project"
```

2. .NET 7.0 projesi oluşturmak için:
```bash
# .NET 7.0 template'ini yükleyin
dotnet new install github.com/korucuoglu/Onion-Architecture-Template#net7.0

# Yeni proje oluşturun
dotnet new korucuoglu-template -o "MyNet7Project"
```

### Template'i Güncelleme

Template'in en son versiyonunu almak için:

```bash
dotnet new update
```

### Template'i Kaldırma

Template'i sistemden kaldırmak isterseniz:

```bash
dotnet new uninstall github.com/korucuoglu/Onion-Architecture-Template
```

Belirli bir branch'in template'ini kaldırmak için:
```bash
dotnet new uninstall github.com/korucuoglu/Onion-Architecture-Template#net7.0
```

## 📁 Klasör Yapısı

```
├── src/
│   ├── MyTemplate.Domain/           # Domain katmanı
│   ├── MyTemplate.Application/      # Uygulama katmanı
│   ├── MyTemplate.Infrastructure/   # Altyapı katmanı
│   ├── MyTemplate.API/             # Web API katmanı
│   └── MyTemplate.WorkerService/   # Worker Service
├── docs/                           # Dokümantasyon
├── .template.config/               # Template yapılandırması
└── tests/                         # Test projeleri
```

## 🛠️ Teknolojiler ve Araçlar

- .NET 9.0+
- Entity Framework Core
- MediatR (CQRS pattern implementasyonu için)
- FluentValidation
- AutoMapper
- Swagger/OpenAPI
- JWT Authentication

## 🔧 Yapılandırma

1. `appsettings.json` dosyasında gerekli yapılandırmaları yapın:
   - Veritabanı bağlantı bilgileri
   - JWT ayarları
   - Logging yapılandırması
   - Diğer uygulama özgü ayarlar

2. Veritabanı migration'larını oluşturun ve uygulayın:
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## 🚦 API Kullanımı

API dokümantasyonuna Swagger UI üzerinden erişebilirsiniz:
```
https://localhost:5001/swagger
```

## 🤝 Katkıda Bulunma

1. Bu repository'yi fork edin
2. Feature branch'i oluşturun (`git checkout -b feature/AmazingFeature`)
3. Değişikliklerinizi commit edin (`git commit -m 'Add some AmazingFeature'`)
4. Branch'inizi push edin (`git push origin feature/AmazingFeature`)
5. Pull Request oluşturun

## 📝 Lisans

Bu proje MIT lisansı altında lisanslanmıştır. Daha fazla bilgi için `LICENSE` dosyasını inceleyebilirsiniz.

## 📞 İletişim

Sorularınız ve önerileriniz için Issues bölümünü kullanabilirsiniz.

---
⭐ Bu template'i beğendiyseniz, yıldız vermeyi unutmayın!

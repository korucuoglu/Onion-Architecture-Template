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

Template'i kullanmak için önce repository'yi klonlamanız ve ardından .NET CLI ile yüklemeniz gerekmektedir:

#### .NET 9.0 için (varsayılan):
```bash
# Repository'yi klonlayın
git clone https://github.com/korucuoglu/Onion-Architecture-Template.git

# Proje dizinine gidin
cd Onion-Architecture-Template

# Template'i yükleyin
dotnet new install .

# Eğer template zaten yüklüyse ve güncellemek istiyorsanız
dotnet new install . --force
```

#### .NET 7.0 için:
```bash
# Repository'yi klonlayın
git clone -b net7.0 https://github.com/korucuoglu/Onion-Architecture-Template.git

# Proje dizinine gidin
cd Onion-Architecture-Template

# Template'i yükleyin
dotnet new install .

# Eğer template zaten yüklüyse ve güncellemek istiyorsanız
dotnet new install . --force
```

> **Not**: 
> - Farklı .NET sürümleri için branch'ler:
>   - `main` branch (varsayılan): .NET 9.0
>   - `net7.0` branch: .NET 7.0
> - `--force` parametresi, eğer template daha önce yüklenmişse üzerine yazılmasını sağlar.
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
# Repository'yi klonlayın ve template'i yükleyin
git clone https://github.com/korucuoglu/Onion-Architecture-Template.git
cd Onion-Architecture-Template
dotnet new install .

# Yeni proje oluşturun
dotnet new korucuoglu-template -o "MyNet9Project"
```

2. .NET 7.0 projesi oluşturmak için:
```bash
# Repository'yi net7.0 branch'i ile klonlayın ve template'i yükleyin
git clone -b net7.0 https://github.com/korucuoglu/Onion-Architecture-Template.git
cd Onion-Architecture-Template
dotnet new install .

# Yeni proje oluşturun
dotnet new korucuoglu-template -o "MyNet7Project"
```

### Template'i Güncelleme

Template'in en son versiyonunu almak için repository'yi güncelleyip yeniden yüklemeniz gerekir:

```bash
# Repository'yi güncelleyin
git pull

# Template'i yeniden yükleyin (var olan template'in üzerine yazar)
dotnet new install . --force
```

### Template'i Kaldırma

Template'i sistemden kaldırmak için:

```bash
dotnet new uninstall Korucuoglu.Template
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

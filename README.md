 ZiraatMatrix 
Kurumsal Proje, Nöbet ve Ekip Kaynak Yönetim Platformu
.NET 9 ve WPF kullanılarak geliştirilmiş; kurumsal ekiplerde proje adam/gün maliyetlerini, çeyreklik efor planlamalarını, ekip izin çakışmalarını ve nöbet çizelgelerini akıllı öneri algoritmalarıyla yöneten N-Katmanlı masaüstü yönetim platformudur.
TEKNOLOJİLER:
C# .NET 9 WPF (XAML) EF Core 9 MS SQL Server N-Tier Architecture Akıllı Algoritmalar Enterprise Resource Management

Smart Executive Digest & Ana Sayfa: Ekip yükü dağılımları, yaklaşan nöbetler ve kritik izin uyarılarını içeren özet panel.

Kullanıcı & Takım Yönetimi: Ekip renk kodları, roller (Analist, Dev, Yönetici) ve personel rehberi.

Akıllı İzin Çakışma Yönetimi: %50 ekip çakışma riski tespiti, özel izin onay akışları ve saatlik/tam gün izin takvimi.

Proje & Adam/Gün Efor Yönetimi: Çeyreklik (Q1-Q4) analist/yazılımcı efor planlama, gerçekleşen harcamalar ve dış firma maliyet takibi.

Ekip İçi Anlık İletişim: Departman/Ekip kanalları ve birebir (Direct Message) mesajlaşma altyapısı.

Projenin Amacı ve Çözdüğü Problemler
Kurumsal yazılım ve operasyon ekiplerinde karşılaşılan temel yönetimsel problemleri tek bir platformda çözmek amacıyla geliştirilmiştir:
Dağınık Efor Takibi: Projelerde analist ve yazılımcı adam/gün (man-day) eforlarının çeyreklik/aylık bazda yanlış planlanmasını engeller.
Kritik İzin Çakışmaları: Aynı anda birden fazla kritik personelin izne çıkması durumunda projelerin aksamasını önleyen %50 Ekip Risk Eşiği kontrolü sunar.
Düzensiz Nöbet Dağılımları: Canlıya geçiş (Release), Firewall ve Sunucu bakım nöbetlerinin adil ve otomatik dağıtılmasını sağlar.
Şeffaf Ekip İletişimi: Proje ve nöbet süreçleri hakkında ekip içi anlık mesajlaşma imkanı verir.

Öne Çıkan Modüller ve Yetenekler

 A. Proje & Efor Yönetimi (Smart Project Allocation)
 
Çeyreklik (Q1-Q4) ve Aylık Matris Planlama: Analist ve yazılımcılar için ay bazlı adam/gün planlaması ve gerçekleşen efor takibi.
Kurumsal Hiyerarşi: GMY (Genel Müdür Yardımcılığı), İş Birimi ve Paydaş Departman kırılımları.
Dış Firma & Maliyet Yönetimi: İç kaynak eforlarının yanı sıra dış firma maliyetlerinin (External Cost) takibi.
Pergel No & Jira Entegrasyonu: Kurumsal talep numaraları ile izlenebilirlik.

 B. İzin Yönetimi & Akıllı Risk Algoritması (Smart Leave Recommendation)
 
Kritik Çakışma Motoru: İzin talebi girildiğinde aynı ekipte aynı roldeki kişilerin çakışma oranını hesaplar. Çakışma %50'yi aşarsa sistemi "Özel İzin Talebi" moduna geçirir.
Saatlik ve Tam Gün İzin: Esnek izin tipleri ve otomatik durum güncellemeleri (Approved / Pending / Rejected).
Yönetici Onay Akışı: Unseen notification sistemi ile yöneticilere anlık izin onay bildirimi düşer.

C. Nöbet & Sürüm Yönetimi (Smart Shift Assistant)

Çoklu Nöbet Tipleri: Aylık Sürüm Nöbetleri, Sunucu/Firewall Geçişleri ve Özel Konu Nöbetleri.
Adil Nöbet Öneri Motoru: Geçmiş nöbet sayılarını analiz ederek nöbeti ekipler arasında eşit dağıtan asistan algoritması.
Harici Bağlantı & Jira Entegrasyonu: Nöbet kayıtlarına Jira bilet no ve yönlendirme linkleri ekleme.

D. Ekip İçi Anlık Mesajlaşma (Team Chat)

Kanal Bazlı İletişim: Genel Kanal, Takım Kanalları (Takip, Tahsis, Teminat vb.) ve Birebir (DM) mesajlaşma.
Okundu Durumu & Bildirim: Mesaj okundu durumlarının takip edilmesi (ChatMessageReadState).

Mimari ve Teknolojik Stack

Proje, kurumsal standartlara uygun olarak N-Katmanlı Mimari (N-Tier Architecture) ile tasarlanmıştır.
ZiraatProje (Solution)
 ├── 🎨 ZiraatProje.UI          --> WPF (XAML), Modern Custom UI Controls, ViewModels & Views
 ├── ⚙️ ZiraatProje.Business    --> Business Logic, Smart/AI Recommendation Engines, Validation
 └── 💾 ZiraatProje.DataAccess  --> Entity Framework Core 9, MS SQL Server, DbInitializer
​
Dil / Platform: C# 13 / .NET 9 (Windows Desktop Platform)

Kullanıcı Arayüzü (UI): WPF (Windows Presentation Foundation), Modern XAML Stilleri, Visual State Manager & Özel Animasyonlar
Veritabanı & ORM: Entity Framework Core 9.0, MS SQL Server LocalDB

Algoritmik Servisler:
SmartLeaveRecommendationService (Ekip İzin Risk Analizi)
SmartShiftAssistantService (Adil Nöbet Dağıtım Motoru)
SmartProjectAllocationService (Matris Efor Hesaplama)
SmartExecutiveDigestService (Yönetici Özet Motoru)

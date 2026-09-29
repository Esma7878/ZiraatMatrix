# 🏛️ ZiraatMatrix

> **Kurumsal Proje, Nöbet ve Ekip Kaynak Yönetim Platformu**  
> .NET 9 ve WPF kullanılarak N-Katmanlı Mimari (N-Tier) prensipleriyle geliştirilmiş; analist ve yazılımcı efor takibini, kritik izin çakışma risklerini ve adil nöbet dağıtımlarını akıllı karar destek algoritmalarıyla yöneten yeni nesil kurumsal masaüstü yönetim sistemi.

---

## 🎯 Projenin Amacı ve Çözülen Problemler

Kurumsal yazılım ve operasyon ekiplerinde karşılaşılan yönetsel darboğazları tek merkezden çözmek üzere geliştirilmiştir:

* **Dağınık Efor ve Kaynak Yönetimi:** Çeyreklik (Q1-Q4) ve aylık bazda analist/yazılımcı adam/gün (man-day) planlamalarının şeffaf, izlenebilir ve kurumsal hiyerarşiye (GMY, İş Birimi, Dış Firma) uygun şekilde yürütülmesi.
* **Kritik İzin Çakışmaları:** Aynı roldeki çalışanların eşzamanlı izin alması durumunda operasyonel risk yaratan senaryoları önleyen **%50 Ekip Risk Eşiği Algoritması**.
* **Düzensiz Nöbet Dağılımları:** Canlıya geçiş (Release), Firewall ve sunucu bakım nöbetlerinin geçmiş veriler analiz edilerek adil ve otomatik şekilde planlanması.
* **Bütünleşik İletişim:** Ekiplerin operasyonel süreçleri ve proje detaylarını harici araçlara ihtiyaç duymadan uygulama içi kanallar üzerinden anlık koordine edebilmesi.

---

## 🚀 Öne Çıkan Modüller ve Yetenekler

### 📊 1. Proje & Efor Yönetimi (Smart Project Allocation)
* **Matris Planlama:** Analist ve yazılımcılar için çeyreklik (Q1-Q4) ve aylık bazda planlanan vs. gerçekleşen adam/gün takibi.
* **Kurumsal Hiyerarşi & Maliyet:** GMY, iş birimi ve paydaş departman kırılımları; iç kaynakların yanı sıra dış firma danışmanlık maliyetlerinin takibi.
* **Kurumsal İzlenebilirlik:** Pergel No ve Jira entegrasyonu ile talep bazlı efor eşleştirme.

### 🛡️ 2. İzin Yönetimi & Akıllı Risk Algoritması (Smart Leave Recommendation)
* **%50 Çakışma Riski Analizi:** Aynı ekipte ve aynı roldeki çalışanların izin taleplerinde operasyonel aksama riskini anlık hesaplama.
* **Dinamik Onay Akışları:** Eşik değeri aşan durumlarda süreci otomatik olarak *"Özel İzin Onayı"* statüsüne yönlendirme.
* **Esnek İzin Yapısı:** Saatlik ve tam gün izin türleri, yönetici anlık bildirimleri ve onay mekanizmaları.

### 📅 3. Nöbet & Sürüm Yönetimi (Smart Shift Assistant)
* **Çoklu Nöbet Desteği:** Canlıya geçiş (Release), sistem/sunucu bakımları ve firewall geçiş nöbetleri.
* **Adil Dağıtım Motoru:** Çalışanların geçmiş nöbet geçmişini, rollerini ve sıklığını analiz ederek nöbetleri ekipler arasında optimize eden akıllı öneri motoru.
* **Jira Entegrasyonu:** Nöbet kayıtlarına doğrudan Jira biletleri ve bağlantı linklerinin tanımlanması.

### 💬 4. Ekip İçi Anlık Mesajlaşma (Team Chat)
* **Kanal Tabanlı İletişim:** Genel duyuru kanalları, takıma özel çalışma odaları (Takip, Tahsis, Teminat vb.) ve uçtan uca birebir (DM) mesajlaşma.
* **Etkileşim:** Okundu bilgisi takibi (`ChatMessageReadState`) ve gerçek zamanlı bildirim yönetimi.

### 📈 5. Yönetici Özet Paneli (Smart Executive Digest)
* Yaklaşan teslimler, kritik eşiğe ulaşan izinler, nöbet takvimi ve ekip kapasite oranlarını tek bir dinamik dashboard üzerinde görselleştirme.

---

## 🛠️ Mimari ve Teknolojik Yığın

Proje, kurumsal ölçeklenebilirlik ve sorumlulukların ayrılığı prensiplerine tam uyum için **N-Katmanlı Mimari (N-Tier)** ile inşa edilmiştir:

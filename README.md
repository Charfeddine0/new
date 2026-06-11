# Fantastic City Generator

## نظرة عامة

هذا المشروع هو مجموعة أدوات Unity لإنشاء مدن واقعية تلقائياً ودمج حركة المرور ونظام النهار/الليل.
وهذا يتضمن: توليد شوارع المدينة، الأبراج، المباني، شبكات الطرق الفرعية، حركة المرور، وتأثيرات النهار/الليل.

> ملاحظة: الملفات `.meta` في المشروع هي ملفات تعريف Unity مرتبطة بالأصول ولا تحتاج تعديل مباشر إلا إذا كنت تعمل داخل محرر Unity.

## البنية الأساسية للمجلدات

- `DayNight/`
  - سكربت ونظام التحكم في تبديل النهار والليل.
  - يحتوي على `DayNight.cs` و `ShiftAtRuntime.cs` بالإضافة إلى `DayNight.prefab` وملفات إعدادات.

- `Documentation/`
  - ملفات وثائق المشروع مثل PDF وملف `ENHANCEMENTS_2026_AR.md`.

- `Editor/`
  - أدوات واجهة Unity Editor.
  - يحتوي على `FCityGenerator.cs` و `TCarEditor.cs` التي توفر نافذة إنشاء المدن وإعدادات حركة المرور.

- `Player/`
  - إعدادات ومكونات تحكم اللاعب لهواتف المحمول أو سطح المكتب.

- `Roads/`
  - أصول الطرق ونماذجها.

- `Scenes/`
  - مشاهد Unity الجاهزة: `Scene-Demo.unity`, `Scene-Mobile.unity`, `Scene-Runtime.unity`.

- `Scripts/`
  - سكربتات المشروع الأساسية.
  - أهم الملفات:
    - `CityGenerator.cs`
    - `TrafficSystem.cs`
    - `CityGenerationProfile.cs`
    - `CityGenerationRequest.cs`
    - `RunTimeSample.cs`
    - `FreeCamera.cs`
    - `RuntimeGenerationControlUI.cs`

- `Shaders/`
  - شيدرات مخصصة مثل `DiffuseAlpha.shader`, `Emissive.shader`, `Reflective.shader`.

- `SkyBox/`
  - أصول السماء والمظلة.

- `Textures/`
  - صور ومواد المشروع.

- `URP Settings/`
  - إعدادات Universal Render Pipeline لمشروع Unity.

- `Water/`
  - أصول الماء وإعدادات التأثيرات.

- `WayTool/`
  - أدوات إنشاء وإدارة مسارات Waypoints.

- `Generate.prefab`
  - عنصر Prefab أساسي لإنشاء المدينة.

## مكونات رئيسية ووظائفها

### CityGenerator (`Scripts/CityGenerator.cs`)

- المسؤول عن إنشاء شبكة شوارع المدينة بأحجام مختلفة:
  - `size=1`: مدينة صغيرة جداً.
  - `size=2`: مدينة صغيرة.
  - `size=3`: مدينة متوسطة.
  - `size=4`: مدينة كبيرة.
- يدعم إنشاء مدن فرعية (`satellite cities`) مع خيارات ترتيبها وربطها بشبكة رئيسية.
- يدعم توليد المباني بعد إنشاء الشوارع.
- يحفظ ملخص التوليد والإحصائيات في `GenerationStats` و `GenerationNetwork`.
- يعتمد على مصفوفات Prefab للأجزاء المختلفة من المدينة مثل `largeBlocks`, `BB`, `BC`, `BR`, `DC`, `EB`, `EC`, `MB`, `BK`, `SB`, `BBS`, `BCS`.

### TrafficSystem (`Scripts/TrafficSystem.cs`)

- يدير حركة المركبات في المدينة باستخدام مسارات `FCGWaypointsContainer`.
- يُنشئ سيارات اصطناعية (`IaCars`) داخل شبكة الطرق.
- يدعم اختيار نظام إشارات المرور يمين أو يسار أو طراز اليابان.
- يوفر وظائف لتحديث جميع نقاط الطريق وإزالة العناصر الفارغة.

### DayNight (`DayNight/DayNight.cs`)

- يدير تبديل النهار/الليل للمشهد.
- يغير المواد والشيدرز عبر القوائم `materialDay` و `materialNight`.
- يبدّل `RenderSettings.skybox` بين `skyBoxDay` و `skyBoxNight`.
- يحدث إعدادات الإضاءة المحيطة والاتجاهية.
- يمكن التبديل بين ضوء الشمس وضوء القمر.

### FCityGenerator Editor Window (`Editor/FCityGenerator.cs`)

- توفر نافذة Unity Editor تحت `Window > Fantastic City Generator`.
- تتيح:
  - اختيار حجم المدينة.
  - إنشاء المدن والشوارع.
  - تمكين أو تعطيل مدن الأقمار الصناعية.
  - التحكم في ترتيب الأقمار الصناعية، النمط العشوائي، أو النقاط المخصصة.
  - حفظ واستيراد الإعدادات إلى/من ملف JSON.
  - إنشاء ملف `CityGenerationProfile` قابل للاستخدام وقت التشغيل.
  - تنفيذ فحوصات صحة المشروع.
  - توليد المباني وإضافة نظام المرور تلقائياً.
  - دمج المجسمات بعد التوليد لتحسين الأداء.

### CityGenerationProfile (`Scripts/CityGenerationProfile.cs`)

- ScriptableObject لحفظ إعدادات التوليد.
- يدعم:
  - حجم المدينة.
  - خيارات شبكات الأقمار الصناعية.
  - ربط الطرق.
  - إعدادات المباني.
  - إعدادات حركة المرور اليمنى/اليسرى.

### CityGenerationRequest (`Scripts/CityGenerationRequest.cs`)

- كلاس بيانات لتعريف طلب التوليد.
- يستخدم وقت التشغيل وأيضاً في `RunTimeSample`.
- يوفر دالة `Normalize()` لضبط القيم ضمن الحدود المقبولة.

### RunTimeSample (`Scripts/RunTimeSample.cs`)

- يستخدم لتشغيل التوليد في وقت التشغيل (Play Mode).
- يدعم:
  - ربط مكونات `CityGenerator` و `TrafficSystem`.
  - استخدام `CityGenerationProfile` عند بدء التشغيل.
  - توليد المدينة والمباني وحركة المرور ديناميكياً.
  - تبديل وضع الليل.

### DayNightEditor (`DayNight/Editor/DayNightEditor.cs`)

- يضيف أزرار تحكم `Day` و `Night` إلى مفتش المكون `DayNight`.
- يتيح تعديل ألوان الشمس والقمر ودرجات السماء مباشرة من محرر Unity.

## كيفية الاستخدام

1. افتح المشروع في Unity.
2. افتح أحد المشاهد من `Scenes/` مثل:
   - `Scene-Demo.unity`
   - `Scene-Mobile.unity`
   - `Scene-Runtime.unity`
3. تأكد من أن المجلدات والأصول موجودة ضمن `Assets/Fantastic City Generator/` إذا كان المشروع داخل مجلد `Assets`.
4. افتح نافذة المولد عبر `Window > Fantastic City Generator`.
5. اضبط:
   - حجم المدينة (`Small`, `Medium`, `Large`, `Very Large`).
   - تمكين `With Sattelite City` إذا أردت مدن فرعية.
   - خيارات ربط الأقمار الصناعية وخيارات التوزيع.
6. اضغط أزرار التوليد:
   - `Generate Setup`
   - `Generate Full Pipeline`
   - `Quick Random`
7. بعد توليد الشوارع، يمكنك توليد المباني عبر `Generate Buildings` أو إضافة المرور عبر `Add Traffic System`.
8. استخدم `DayNight` لتغيير مظهر المشهد بين النهار والليل.

## نقاط مهمة

- إذا أضفت `TrafficSystem`، تأكد من تعيين مصفوفة `IaCars` في المفتش وإعداد `maxVehiclesWithPlayer`.
- عند استخدام مدن الأقمار الصناعية، يجب أن تتوفر أصول `miniBorderWithExitOfCity`, `smallBorderWithExitOfCity`, `mediumBorderWithExitOfCity`, أو `largeBorderWithExitOfCity` حسب الحجم.
- يمكنك حفظ حالة الأداة الحالية في الجلسة، أو تصدير إعدادات إلى JSON، أو إنشاء ملف ملف تعريف وقت التشغيل.
- يستخدم المشروع `URP` حسب وجود مجلد `URP Settings`، لذا من الأفضل استخدام إعدادات Pipeline المناسبة في Unity.

## توصيات

- عند توليد مشهد جديد، استخدم `Clear Streets` ثم `Generate Full Pipeline` لتأكيد البدء من جديد.
- إن أردت نتائج متسقة، فعّل `Use Fixed Seed` أو `useCitySeed` في إعدادات التوليد.
- لتوليد مدن أكبر أو أكثر تعقيداً، اضبط القيم الخاصة بالأقمار الصناعية والاتصال الشبكي.

## ملفات إضافية

- `Nouveau Document texte.txt` هو ملف نصي بسيط ربما ملاحظة أو اختبار.
- ملفات `.meta` هي ملفات تعريف Unity التي تخزن معرفات الـ GUID والروابط الداخلية.
- مجلد `Documentation/` يحوي مواد إضافية قد تشرح الواجهة أو الاستخدامات.

## الخلاصة

هذا مشروع Unity مخصّص لإنشاء المدن تلقائياً مع نظام يوم/ليل وحركة مرور ذكية.
يمكن استخدامه عبر محرر Unity أو في وقت التشغيل بواسطة `RunTimeSample`، ويتيح حفظ إعدادات التوليد وإعادة استخدامها بسهولة.
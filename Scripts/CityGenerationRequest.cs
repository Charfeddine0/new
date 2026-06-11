using System.Collections.Generic;
using UnityEngine;

namespace FCG
{
    /// <summary>
    /// طلب توليد المدينة - يحتوي على جميع إعدادات التوليد الإجرائي
    /// </summary>
    /// <remarks>
    /// تُستخدم هذه الفئة لتمرير جميع معاملات التوليد كوحدة واحدة، مما يسهل:
    /// - التسلسل والاسترجاع من ملفات الإعدادات
    /// - التمرير بين مكونات مختلفة
    /// - إعادة تشغيل نفس الإعدادات
    /// </remarks>
    [System.Serializable]
    public class CityGenerationRequest
    {
        /// <summary>حجم المدينة (1=صغير جداً، 4=كبير جداً)</summary>
        [Range(1, 4)] public int citySize = 3;
        
        /// <summary>إضافة مدن فضائية حول المدينة الرئيسية</summary>
        public bool withSatelliteCity = false;
        
        /// <summary>استخدام حدود مسطحة بدلاً من المعقدة</summary>
        public bool borderFlat = false;
        
        /// <summary>عدد المدن الفضائية (إذا كانت مفعلة)</summary>
        [Range(1, 64)] public int satelliteCityCount = 1;
        
        /// <summary>ربط المدن الفضائية بالمدينة الرئيسية</summary>
        public bool connectSatellitesToMain = true;
        
        /// <summary>ربط المدن الفضائية ببعضها البعض</summary>
        public bool connectSatellitesTogether = true;
        
        /// <summary>استخدام أحجام عشوائية للمدن الفضائية</summary>
        public bool randomSatelliteSizes = false;
        
        /// <summary>الحد الأدنى لحجم المدينة الفضائية</summary>
        [Range(1, 4)] public int satelliteCityMinSize = 1;
        
        /// <summary>الحد الأقصى لحجم المدينة الفضائية</summary>
        [Range(1, 4)] public int satelliteCityMaxSize = 1;
        
        /// <summary>استخدام توزيع عشوائي للمدن الفضائية بدلاً من المواضع المعرفة مسبقاً</summary>
        public bool randomSatelliteLayout = false;
        
        /// <summary>استخدام بذرة عشوائية لضمان نفس النتائج في كل مرة</summary>
        public bool useCitySeed = false;
        
        /// <summary>بذرة المدينة العشوائية</summary>
        public int citySeed = 123456;
        
        /// <summary>حساب عدد المدن الفضائية تلقائياً بناءً على حجم المدينة</summary>
        public bool autoSatelliteCount = false;
        
        /// <summary>استخدام بذرة عشوائية للمدن الفضائية</summary>
        public bool useSatelliteSeed = false;
        
        /// <summary>بذرة المدن الفضائية العشوائية</summary>
        public int satelliteSeed = 12345;
        
        /// <summary>الحد الأدنى لموضع المدينة الفضائية (X)</summary>
        public Vector2 randomSatelliteMin = new Vector2(-1000f, -2200f);
        
        /// <summary>الحد الأقصى لموضع المدينة الفضائية (X)</summary>
        public Vector2 randomSatelliteMax = new Vector2(1000f, -1200f);
        
        /// <summary>مواضع مخصصة للمدن الفضائية</summary>
        public List<Vector2> customSatelliteOffsets = new List<Vector2>();
        
        /// <summary>استخدام المواضع المخصصة بدلاً من العشوائية أو المعرفة</summary>
        public bool useCustomSatelliteOffsets = false;
        
        /// <summary>إزاحة عامة لجميع المدن الفضائية</summary>
        public Vector2 satelliteGlobalOffset = Vector2.zero;
        
        /// <summary>طريقة ربط المدن الفضائية (Chain, Nearest, FullMesh)</summary>
        public CityGenerator.SatelliteConnectionMode satelliteConnectionMode = CityGenerator.SatelliteConnectionMode.Chain;
        
        /// <summary>الحد الأقصى لعدد الروابط لكل مدينة فضائية</summary>
        [Range(1, 6)] public int satelliteMaxNeighborLinks = 1;
        
        /// <summary>إغلاق الحلقة بربط آخر مدينة بالأولى</summary>
        public bool satelliteCloseLoop = false;
        
        /// <summary>تجاوز خطوة الاتصال الافتراضية</summary>
        public float connectionStepOverride = 0f;
        
        /// <summary>إنشاء نقاط مرجعية للشبكة المولدة</summary>
        public bool createCityAnchors = true;
        
        /// <summary>توليد المباني تلقائياً بعد توليد الشوارع</summary>
        public bool autoGenerateBuildings = true;
        
        /// <summary>إضافة منطقة وسط البلد (مباني عالية)</summary>
        public bool withDownTownArea = true;
        
        /// <summary>حجم منطقة وسط البلد</summary>
        [Range(50, 200)] public float downTownSize = 100f;
        
        /// <summary>إنشاء خطوط debug لعرض الاتصالات</summary>
        public bool createConnectionDebugLines = false;
        
        /// <summary>ارتفاع خطوط debug</summary>
        public float connectionDebugLineHeight = 3f;
        
        /// <summary>كثافة المباني في المدن الفضائية (0-1)</summary>
        [Range(0f, 1f)] public float satelliteBuildingDensity = 1f;
        
        /// <summary>عدد أعمدة خريطة المدن (للتوليد المتعدد)</summary>
        public int mapColumns = 1;
        
        /// <summary>عدد صفوف خريطة المدن (للتوليد المتعدد)</summary>
        public int mapRows = 1;
        
        /// <summary>المسافة الأفقية بين خرائط المدن</summary>
        public float mapSpacingX = 2500f;
        
        /// <summary>المسافة الرأسية بين خرائط المدن</summary>
        public float mapSpacingZ = 2500f;

        /// <summary>
        /// تطبيع القيم المخارجة عن الحدود المسموحة
        /// </summary>
        /// <remarks>
        /// تُستدعى هذه الطريقة تلقائياً قبل التوليد لضمان أن جميع القيم ضمن الحدود المسموحة.
        /// على سبيل المثال:
        /// - citySize بين 1-4
        /// - satelliteCityCount بين 1-64
        /// - الحد الأدنى لا يتجاوز الحد الأقصى
        /// </remarks>
        public void Normalize()
        {
            citySize = Mathf.Clamp(citySize, 1, 4);
            satelliteCityCount = Mathf.Clamp(satelliteCityCount, 1, 64);
            satelliteMaxNeighborLinks = Mathf.Clamp(satelliteMaxNeighborLinks, 1, 6);
            satelliteCityMinSize = Mathf.Clamp(satelliteCityMinSize, 1, 4);
            satelliteCityMaxSize = Mathf.Clamp(satelliteCityMaxSize, 1, 4);
            mapColumns = Mathf.Max(1, mapColumns);
            mapRows = Mathf.Max(1, mapRows);
            mapSpacingX = Mathf.Max(0f, mapSpacingX);
            mapSpacingZ = Mathf.Max(0f, mapSpacingZ);
            connectionStepOverride = Mathf.Max(0f, connectionStepOverride);
            connectionDebugLineHeight = Mathf.Clamp(connectionDebugLineHeight, 0f, 20f);

            if (randomSatelliteMin.x > randomSatelliteMax.x)
            {
                float t = randomSatelliteMin.x;
                randomSatelliteMin.x = randomSatelliteMax.x;
                randomSatelliteMax.x = t;
            }
            // Ensure seed values are non-negative
            if (citySeed < 0) citySeed = 0;
            if (satelliteSeed < 0) satelliteSeed = 0;

            if (satelliteCityMinSize > satelliteCityMaxSize)
            {
                int t = satelliteCityMinSize;
                satelliteCityMinSize = satelliteCityMaxSize;
                satelliteCityMaxSize = t;
            }

            if (randomSatelliteMin.y > randomSatelliteMax.y)
            {
                float t = randomSatelliteMin.y;
                randomSatelliteMin.y = randomSatelliteMax.y;
                randomSatelliteMax.y = t;
            }
            if (satelliteCityMinSize > satelliteCityMaxSize)
            {
                int t = satelliteCityMinSize;
                satelliteCityMinSize = satelliteCityMaxSize;
                satelliteCityMaxSize = t;
            }

            downTownSize = Mathf.Clamp(downTownSize, 50f, 200f);
        }

        /// <summary>
        /// التحقق الشامل من صحة جميع الإعدادات
        /// </summary>
        /// <exception cref="ConfigurationException">إذا كانت أي قيمة غير صحيحة</exception>
        /// <remarks>
        /// تُرفع هذه الطريقة استثناءات بدلاً من مجرد إرجاع false/null
        /// مما يجعل البرنامج يفشل بسرعة ووضوح عند وجود مشكلة
        /// </remarks>
        public void Validate()
        {
            // التحقق من نطاق citySize
            if (citySize < 1 || citySize > 4)
                throw new ConfigurationException(
                    $"citySize يجب أن يكون 1-4، لكنه الآن {citySize}",
                    nameof(citySize)
                );

            // التحقق من عدد المدن الفضائية
            if (satelliteCityCount < 1 || satelliteCityCount > 64)
                throw new ConfigurationException(
                    $"satelliteCityCount يجب أن يكون 1-64، لكنه الآن {satelliteCityCount}",
                    nameof(satelliteCityCount)
                );

            // التحقق من أحجام المدن الفضائية
            if (satelliteCityMinSize < 1 || satelliteCityMinSize > 4)
                throw new ConfigurationException(
                    $"satelliteCityMinSize يجب أن يكون 1-4، لكنه الآن {satelliteCityMinSize}",
                    nameof(satelliteCityMinSize)
                );

            if (satelliteCityMaxSize < 1 || satelliteCityMaxSize > 4)
                throw new ConfigurationException(
                    $"satelliteCityMaxSize يجب أن يكون 1-4، لكنه الآن {satelliteCityMaxSize}",
                    nameof(satelliteCityMaxSize)
                );

            // التحقق من أن minSize <= maxSize
            if (satelliteCityMinSize > satelliteCityMaxSize)
                throw new ConfigurationException(
                    $"satelliteCityMinSize ({satelliteCityMinSize}) يجب أن يكون <= satelliteCityMaxSize ({satelliteCityMaxSize})",
                    nameof(satelliteCityMinSize)
                );

            // التحقق من خريطة المدن
            if (mapColumns < 1 || mapRows < 1)
                throw new ConfigurationException(
                    $"mapColumns و mapRows يجب أن تكون >= 1",
                    "mapGrid"
                );

            // التحقق من تباعد الخرائط
            if (mapSpacingX < 0 || mapSpacingZ < 0)
                throw new ConfigurationException(
                    $"mapSpacingX و mapSpacingZ يجب أن تكونا >= 0",
                    "mapSpacing"
                );

            // التحقق من downTownSize
            if (downTownSize < 50f || downTownSize > 200f)
                throw new ConfigurationException(
                    $"downTownSize يجب أن يكون 50-200، لكنه الآن {downTownSize}",
                    nameof(downTownSize)
                );

            // التحقق من satelliteMaxNeighborLinks
            if (satelliteMaxNeighborLinks < 1 || satelliteMaxNeighborLinks > 6)
                throw new ConfigurationException(
                    $"satelliteMaxNeighborLinks يجب أن يكون 1-6، لكنه الآن {satelliteMaxNeighborLinks}",
                    nameof(satelliteMaxNeighborLinks)
                );
        }

        /// <summary>
        /// إنشاء طلب توليد من ملف إعدادات مُحفوظ
        /// </summary>
        /// <param name="profile">ملف الإعدادات (Profile Asset)</param>
        /// <returns>طلب توليد جديد يحتوي على جميع إعدادات الملف</returns>
        /// <remarks>
        /// إذا كان ملف الإعدادات null، يتم إرجاع إعدادات افتراضية جديدة.
        /// يتم تطبيع جميع القيم تلقائياً قبل الإرجاع.
        /// </remarks>
        public static CityGenerationRequest FromProfile(CityGenerationProfile profile)
        {
            if (!profile)
                return new CityGenerationRequest();

            CityGenerationRequest request = new CityGenerationRequest();
            request.citySize = profile.citySize;
            request.withSatelliteCity = profile.withSatelliteCity;
            request.borderFlat = profile.borderFlat;
            request.satelliteCityCount = profile.satelliteCityCount;
            request.connectSatellitesToMain = profile.connectSatellitesToMain;
            request.connectSatellitesTogether = profile.connectSatellitesTogether;
            request.satelliteConnectionMode = profile.satelliteConnectionMode;
            request.satelliteMaxNeighborLinks = profile.satelliteMaxNeighborLinks;
            request.satelliteCloseLoop = profile.satelliteCloseLoop;
            request.connectionStepOverride = profile.satelliteConnectionStep;
            request.createCityAnchors = profile.createCityAnchors;
            request.autoGenerateBuildings = profile.autoGenerateBuildings;
            request.withDownTownArea = profile.withDownTownArea;
            request.downTownSize = profile.downTownSize;
            request.createConnectionDebugLines = profile.createConnectionDebugLines;
            request.connectionDebugLineHeight = profile.connectionDebugLineHeight;
            request.satelliteBuildingDensity = profile.satelliteBuildingDensity;
            request.randomSatelliteLayout = profile.randomSatelliteLayout;
            request.useSatelliteSeed = profile.useSatelliteSeed;
            request.satelliteSeed = profile.satelliteSeed;
            request.useCitySeed = profile.useCitySeed;
            request.citySeed = profile.citySeed;
            request.autoSatelliteCount = profile.autoSatelliteCount;
            request.randomSatelliteMin = profile.randomSatelliteMin;
            request.randomSatelliteMax = profile.randomSatelliteMax;
            request.satelliteGlobalOffset = profile.satelliteGlobalOffset;
            request.customSatelliteOffsets = (profile.customSatelliteOffsets != null)
                ? new List<Vector2>(profile.customSatelliteOffsets)
                : new List<Vector2>();
            request.useCustomSatelliteOffsets = !request.randomSatelliteLayout && request.customSatelliteOffsets.Count > 0;
            request.randomSatelliteSizes = profile.randomSatelliteSizes;
            request.mapColumns = profile.mapColumns;
            request.mapRows = profile.mapRows;
            request.mapSpacingX = profile.mapSpacingX;
            request.mapSpacingZ = profile.mapSpacingZ;
            request.satelliteCityMinSize = profile.satelliteCityMinSize;
            request.satelliteCityMaxSize = profile.satelliteCityMaxSize;
            request.Normalize();
            return request;
        }
    }
}

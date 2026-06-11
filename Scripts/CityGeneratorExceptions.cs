using System;
using System.Collections.Generic;
using UnityEngine;

namespace FCG
{
    /// <summary>
    /// الفئة الأساسية لجميع استثناءات مولد المدينة
    /// </summary>
    /// <remarks>
    /// تُستخدم هذه الفئة كـ base class لجميع الاستثناءات المخصصة في نظام التوليد.
    /// تسمح بالتمييز بسهولة بين أخطاء المشروع والأخطاء الأخرى.
    /// </remarks>
    public class CityGeneratorException : Exception
    {
        /// <summary>رمز الخطأ المميز</summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// إنشاء استثناء جديد
        /// </summary>
        public CityGeneratorException(string message, string errorCode = "UNKNOWN") 
            : base(message)
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// إنشاء استثناء مع inner exception
        /// </summary>
        public CityGeneratorException(string message, Exception innerException, string errorCode = "UNKNOWN")
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }

    /// <summary>
    /// استثناء يُرفع عندما تكون الأصول (Assets) المطلوبة غير موجودة أو غير صحيحة
    /// </summary>
    /// <remarks>
    /// يحدث عندما:
    /// - مصفوفة prefabs فارغة
    /// - نوع من الـ borders غير موجود
    /// - عدد الأصول غير كافٍ
    /// 
    /// مثال:
    /// <code>
    /// if (largeBlocks == null || largeBlocks.Length == 0)
    ///     throw new AssetValidationException(
    ///         "لم يتم تحميل largeBlocks",
    ///         new List<string> { "largeBlocks" }
    ///     );
    /// </code>
    /// </remarks>
    public class AssetValidationException : CityGeneratorException
    {
        /// <summary>قائمة الأصول المفقودة</summary>
        public List<string> MissingAssets { get; set; }

        /// <summary>
        /// إنشاء استثناء تحقق الأصول
        /// </summary>
        /// <param name="message">رسالة الخطأ</param>
        /// <param name="missingAssets">قائمة الأصول المفقودة</param>
        public AssetValidationException(string message, List<string> missingAssets = null)
            : base(message, "ASSET_VALIDATION_FAILED")
        {
            MissingAssets = missingAssets ?? new List<string>();
        }

        /// <summary>
        /// الحصول على رسالة خطأ مفصلة
        /// </summary>
        public override string ToString()
        {
            string baseMessage = base.ToString();
            if (MissingAssets.Count > 0)
                baseMessage += $"\nالأصول المفقودة: {string.Join(", ", MissingAssets)}";
            return baseMessage;
        }
    }

    /// <summary>
    /// استثناء يُرفع عندما تكون الإعدادات (Configuration) غير صحيحة أو غير متوافقة
    /// </summary>
    /// <remarks>
    /// يحدث عندما:
    /// - قيمة حجم المدينة خارج النطاق (ليست 1-4)
    /// - عدد المدن الفضائية سالب
    /// - الإعدادات متناقضة (مثل minSize > maxSize)
    /// 
    /// مثال:
    /// <code>
    /// if (citySize < 1 || citySize > 4)
    ///     throw new ConfigurationException(
    ///         "حجم المدينة يجب أن يكون بين 1 و 4",
    ///         "INVALID_CITY_SIZE"
    ///     );
    /// </code>
    /// </remarks>
    public class ConfigurationException : CityGeneratorException
    {
        /// <summary>الحقل المسبب للمشكلة</summary>
        public string ParameterName { get; set; }

        /// <summary>
        /// إنشاء استثناء الإعدادات
        /// </summary>
        /// <param name="message">رسالة الخطأ</param>
        /// <param name="parameterName">اسم المعامل الخاطئ</param>
        public ConfigurationException(string message, string parameterName = null)
            : base(message, "CONFIGURATION_ERROR")
        {
            ParameterName = parameterName;
        }
    }

    /// <summary>
    /// استثناء يُرفع عندما يفشل التوليد أثناء العملية
    /// </summary>
    /// <remarks>
    /// يحدث عندما:
    /// - فشل إنشاء الشوارع
    /// - فشل إنشاء المباني
    /// - فشل الاتصال بين المدن
    /// - تجاوز وقت التنفيذ
    /// 
    /// مثال:
    /// <code>
    /// try
    /// {
    ///     GenerateStreetsVerySmall(borderFlat, withSatelliteCity);
    /// }
    /// catch (Exception ex)
    /// {
    ///     throw new GenerationException(
    ///         "فشل توليد الشوارع",
    ///         ex,
    ///         "STREET_GENERATION_FAILED"
    ///     );
    /// }
    /// </code>
    /// </remarks>
    public class GenerationException : CityGeneratorException
    {
        /// <summary>مرحلة التوليد التي فشلت</summary>
        public string GenerationPhase { get; set; }

        /// <summary>
        /// إنشاء استثناء التوليد
        /// </summary>
        /// <param name="message">رسالة الخطأ</param>
        /// <param name="generationPhase">المرحلة التي فشلت</param>
        public GenerationException(string message, string generationPhase = null)
            : base(message, "GENERATION_FAILED")
        {
            GenerationPhase = generationPhase;
        }

        /// <summary>
        /// إنشاء استثناء التوليد مع inner exception
        /// </summary>
        public GenerationException(string message, Exception innerException, string generationPhase = null)
            : base(message, innerException, "GENERATION_FAILED")
        {
            GenerationPhase = generationPhase;
        }
    }

    /// <summary>
    /// استثناء يُرفع عندما تكون حالة الكائن غير صحيحة للعملية المطلوبة
    /// </summary>
    /// <remarks>
    /// يحدث عندما:
    /// - محاولة توليد بدون تحميل الأصول
    /// - محاولة توليد أثناء توليد آخر
    /// - الكائن غير مهيأ بشكل صحيح
    /// </remarks>
    public class InvalidStateException : CityGeneratorException
    {
        /// <summary>
        /// إنشاء استثناء حالة غير صحيحة
        /// </summary>
        public InvalidStateException(string message)
            : base(message, "INVALID_STATE")
        {
        }
    }

    /// <summary>
    /// فئة مساعدة للتحقق من الصحة والتنسيق
    /// </summary>
    /// <remarks>
    /// توفر طرق مساعدة لـ validation والتحقق من الشروط
    /// </remarks>
    public static class ValidationHelper
    {
        /// <summary>
        /// التحقق من أن المصفوفة ليست فارغة
        /// </summary>
        /// <param name="array">المصفوفة المراد التحقق منها</param>
        /// <param name="arrayName">اسم المصفوفة (للرسالة)</param>
        /// <exception cref="AssetValidationException">إذا كانت المصفوفة فارغة</exception>
        public static void ValidateArrayNotEmpty(GameObject[] array, string arrayName)
        {
            if (array == null || array.Length == 0)
            {
                throw new AssetValidationException(
                    $"المصفوفة '{arrayName}' فارغة أو غير محملة",
                    new List<string> { arrayName }
                );
            }
        }

        /// <summary>
        /// التحقق من نطاق القيمة
        /// </summary>
        /// <param name="value">القيمة المراد التحقق منها</param>
        /// <param name="min">الحد الأدنى</param>
        /// <param name="max">الحد الأقصى</param>
        /// <param name="parameterName">اسم المعامل</param>
        /// <exception cref="ConfigurationException">إذا كانت القيمة خارج النطاق</exception>
        public static void ValidateRange(int value, int min, int max, string parameterName)
        {
            if (value < min || value > max)
            {
                throw new ConfigurationException(
                    $"{parameterName} يجب أن يكون بين {min} و {max}، لكنه الآن {value}",
                    parameterName
                );
            }
        }

        /// <summary>
        /// التحقق من أن القيمة موجبة
        /// </summary>
        public static void ValidatePositive(float value, string parameterName)
        {
            if (value < 0)
            {
                throw new ConfigurationException(
                    $"{parameterName} يجب أن تكون موجبة، لكنها الآن {value}",
                    parameterName
                );
            }
        }

        /// <summary>
        /// التحقق من أن Bounds متوافقة
        /// </summary>
        public static void ValidateBounds(float min, float max, string parameterName)
        {
            if (min > max)
            {
                throw new ConfigurationException(
                    $"الحد الأدنى ({min}) يجب أن يكون أصغر من الحد الأقصى ({max}) في {parameterName}",
                    parameterName
                );
            }
        }

        /// <summary>
        /// التحقق من الشرط
        /// </summary>
        public static void Assert(bool condition, string message, string errorCode = "ASSERTION_FAILED")
        {
            if (!condition)
            {
                throw new CityGeneratorException(message, errorCode);
            }
        }
    }
}

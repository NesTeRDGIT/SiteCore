using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace SiteCore.Class
{
    /// <summary>
    /// Тип сравнения
    /// </summary>
    public enum CompareExAttributeType
    {
        /// <summary>
        /// Более
        /// </summary>
        More,
        /// <summary>
        /// Более или равно
        /// </summary>
        MoreOrEqual,
        /// <summary>
        /// Менее
        /// </summary>
        Less,
        /// <summary>
        /// Менее или равно
        /// </summary>
        LessOrEqual
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class CompareExAttribute : ValidationAttribute
    {
        #region Properties

        /// <summary>
        /// Поле для сравнения
        /// </summary>
        public string OtherProperty { get; }

        /// <summary>
        /// Наименование поля для сравнения
        /// </summary>
        public string OtherPropertyDisplayName { get; set; }

        /// <summary>
        /// Тип сравнения
        /// </summary>

        public CompareExAttributeType Type { get; }

        /// <summary>
        /// Не сравнивать поле если значение равно NULL
        /// </summary>
        public bool isNull { get; }


        public override bool RequiresValidationContext => true;

        #endregion

        #region Constructor

        /// <summary>
        /// Атрибут сравнения
        /// </summary>
        /// <param name="otherProperty">Поле для сравнения</param>
        /// <param name="otherPropertyDisplayName">Наименование поля для сравнения</param>
        /// <param name="type">Тип сравнения</param>
        /// <param name="isNull">Не проверять поле если значение равно null</param>
        /// <param name="comparer">Comparer для сравнения</param>
        public CompareExAttribute(string otherProperty, string otherPropertyDisplayName, CompareExAttributeType type, bool isNull) : base("Значение поля '{0}' должно быть {1} чем значение поля {2}'.")
        {
            this.OtherProperty = otherProperty;
            this.OtherPropertyDisplayName = otherPropertyDisplayName;
            this.Type = type;
            this.isNull = isNull;
        
        }

        #endregion

        /// <summary>
        /// Applies formatting to an error message, based on the data field where the error occurred.
        /// </summary>
        /// <param name="name">The name to include in the formatted message.</param>
        /// <returns>
        /// An instance of the formatted error message.
        /// </returns>
        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name, TypeToString(Type), OtherPropertyDisplayName ?? OtherProperty);
        }


        private string TypeToString(CompareExAttributeType type)
        {
            return type switch
            {
                CompareExAttributeType.More => "больше",
                CompareExAttributeType.MoreOrEqual => "больше или равно",
                CompareExAttributeType.Less => "меньше",
                CompareExAttributeType.LessOrEqual => "меньше или равно",
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        private IComparer findComparer(Type type)
        {
            if (type == typeof(DateTime))
                return new DateTimeComparer();
            throw new Exception($"Не поддерживаемый  тип данных {type.Name}");
        }

        /// <summary>
        /// Validates the specified value with respect to the current validation attribute.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>
        /// An instance of the <see cref="T:System.ComponentModel.DataAnnotations.ValidationResult" /> class.
        /// </returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (isNull && value == null)
                return ValidationResult.Success;

            if (validationContext == null)
            {
                throw new ArgumentNullException(nameof(validationContext));
            }

            var otherProperty = validationContext.ObjectType.GetProperty(this.OtherProperty);
            if (otherProperty == null)
            {
                return new ValidationResult($"Не удалось найти поле '{this.OtherProperty}'.");
            }

            var Comparer = findComparer(otherProperty.PropertyType);

            var otherValue = otherProperty.GetValue(validationContext.ObjectInstance);

            switch (Type)
            {
                case CompareExAttributeType.More:
                    if (Comparer.Compare(value, otherValue) <= 0)
                        return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
                    break;
                case CompareExAttributeType.MoreOrEqual:
                    if (Comparer.Compare(value, otherValue) < 0)
                        return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
                    break;
                case CompareExAttributeType.Less:
                    if (Comparer.Compare(value, otherValue) >= 0)
                        return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
                    break;
                case CompareExAttributeType.LessOrEqual:
                    if (Comparer.Compare(value, otherValue) > 0)
                        return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return ValidationResult.Success;
        }
    }

    public class DateTimeComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            if (x != null && y != null)
                return DateTime.Compare((DateTime)x, (DateTime)y);
            throw new Exception("Не допускается сравнения элементов значения null");
        }
    }

}

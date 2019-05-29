﻿using System.Globalization;
using WXKJ.Framework.Helpers;
using WXKJ.Framework.Properties;
using WXKJ.Framework.Validations.Validators;
using WXKJ.Framework.Extensions;
using WXKJ.Framework.Util.Extensions;

namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// 身份证验证
    /// </summary>
    [AttributeUsage( AttributeTargets.Property )]
    public class IdCardAttribute : ValidationAttribute {
        /// <summary>
        /// 格式化错误消息
        /// </summary>
        public override string FormatErrorMessage( string name ) {
            if( ErrorMessage == null && ErrorMessageResourceName == null )
                ErrorMessage = LibraryResource.InvalidIdCard;
            return String.Format( CultureInfo.CurrentCulture, ErrorMessageString );
        }

        /// <summary>
        /// 是否验证通过
        /// </summary>
        protected override ValidationResult IsValid( object value, ValidationContext validationContext ) {
            if( value.SafeString().IsEmpty() )
                return ValidationResult.Success;
            if( Regex.IsMatch( value.SafeString(), ValidatePattern.IdCardPattern ) )
                return ValidationResult.Success;
            return new ValidationResult( FormatErrorMessage( string.Empty ) );
        }
    }
}
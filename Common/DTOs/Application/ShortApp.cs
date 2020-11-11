using System;
using System.ComponentModel.DataAnnotations;

namespace Common.DTOs.Application
{
    public class ShortApp
    {
        #region Private

        private string _timeAtResidenceYears;
        private string _timeAtResidenceMonths;
        private string _timeAtJobYears;
        private string _timeAtJobMonths;

        private decimal? _otherIncome;
        private decimal? _netPaycheck;

        #endregion Private

        #region Public

        [MaxLength(50)]
        [Required]
        public string FirstName { get; set; }

        //[AllowEmpty]
        [MaxLength(50)]
        public string MiddleName { get; set; }

        [MaxLength(50)]
        [Required]
        public string LastName { get; set; }

        [RegularExpression(@"(^$)|(^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$)")]
        //[RequiredIf("Email", "")]
        public string PhoneNumber { get; set; }

        //[AllowEmpty]
        public string PhoneType { get; set; }

        [DataType(DataType.EmailAddress)]
        //[RequiredIf("PhoneNumber", "")]
        public string Email { get; set; }

        public string HouseNumber { get; set; }

        //[AllowEmpty]
        [MaxLength(100)]
        public string ApartmentNumber { get; set; }

        [MaxLength(100)]
        public string StreetName { get; set; }

        public int? StreetTypeId { get; set; }

        [MaxLength(100)]
        public string City { get; set; }

        [DataType(DataType.PostalCode)]
        public string Zip { get; set; }

        public int? StateId { get; set; }

        public int? HouseTypeId { get; set; }

        //[AllowEmpty]
        public string Referrer { get; set; }

        public string TimeAtResidenceYears
        {
            get
            {
                return _timeAtResidenceYears;
            }
            set
            {
                _timeAtResidenceYears = value?.ToLowerInvariant().Replace(" year", String.Empty).Replace("s", String.Empty);
            }
        }

        public string TimeAtResidenceMonths
        {
            get
            {
                return _timeAtResidenceMonths;
            }
            set
            {
                _timeAtResidenceMonths = value?.ToLowerInvariant().Replace(" month", String.Empty).Replace("s", String.Empty);
            }
        }

        public string TimeAtJobYears
        {
            get
            {
                return _timeAtJobYears;
            }
            set
            {
                _timeAtJobYears = value?.ToLowerInvariant().Replace(" year", String.Empty).Replace("s", String.Empty);
            }
        }

        public string TimeAtJobMonths
        {
            get
            {
                return _timeAtJobMonths;
            }
            set
            {
                _timeAtJobMonths = value?.ToLowerInvariant().Replace(" month", String.Empty).Replace("s", String.Empty);
            }
        }

        public decimal? NetPeriodPaycheck
        {
            get
            {
                return _netPaycheck;
            }
            set
            {
                _netPaycheck = value;
            }
        }

        public int? PaymentTypeId { get; set; }

        public decimal? OtherIncome
        {
            get
            {
                return _otherIncome;
            }
            set
            {
                _otherIncome = value;
            }
        }

        public int? OtherIncomePayPeriodId { get; set; }

        //[AllowEmpty]
        public bool ActiveOrFormerMilitary { get; set; }

        //[AllowEmpty]
        public int MilitaryChoise { get; set; }

        [RegularExpression(@"(^$)|(^\d{9}|\d{3}-\d{2}-\d{4}$)")]
        public string Ssn { get; set; }

        public bool? CurrentlyInBankruptcy { get; set; }

        #endregion Public

    }
}

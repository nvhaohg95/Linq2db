




/* LV Business Solutions Center
 * Copyright (c) 2023 Lac Viet
 * http://www.lacviet.com.vn
 *=============================================================
 * File name            : AD_Users.cs          
 * Created by           : Auto - 12/01/2023 08:31:29            
 * Last modify          : Auto - 12/01/2023 08:31:29            
 * Version              : 1.0                                  
 * ============================================================
 */

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Linq2dbContext.Model
{
    [Serializable]
    [Table("AD_Users")]
    public class AD_Users
    {
        #region Properties

        public AD_Users()
        {
            RecID = Guid.NewGuid();
            UserType = "0";
            IsSystem = false;
            Administrator = false;
            SystemAdmin = false;
            FunctionAdmin = false;
            Password = "";
            NeverExpire = false;
            FirstChange = false;
            CantChange = false;
            SystemFormat = "1";
            ActiveTrackLog = false;
            ActiveAuditTrail = false;
            PersonalizeView = false;
            PrintConsolidate = false;
            AutoRefresh = 0;
            Disconnect = 0;
            DefaultPath = "1";
            LoginSystem = "1";
            FailedPWAttempt = 0;
            FailedAnswerAttempt = 0;
            IsOnline = false;
            IsLockedOut = false;
            Latitude = 0;
            Longitude = 0;
            Stop = false;
            CreatedOn = DateTime.Now;
            CreatedBy = "IMPORTED";
        }


        [Required]
        public Guid RecID { get; set; }

        [Key]
        [Column(Order = 2)]
        [Required]
        [MaxLength(20)]
        public string UserID { get; set; }

        [MaxLength(1)]
        public string UserType { get; set; }

        [MaxLength(200)]
        public string UserName { get; set; }

        [MaxLength(100)]
        public string LastName { get; set; }

        [MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(200)]
        public string SearchName { get; set; }

        [MaxLength(400)]
        public string Description { get; set; }

        [MaxLength(50)]
        public string DomainUser { get; set; }

        [MaxLength(150)]
        public string Email { get; set; }

        [MaxLength(150)]
        public string PersonalEmail { get; set; }

        [MaxLength(20)]
        public string Mobile { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }

        [MaxLength(500)]
        public string Picture { get; set; }

        [MaxLength(20)]
        public string SLALevel { get; set; }

        [MaxLength(1)]
        public string SLA24x7 { get; set; }

        [MaxLength(20)]
        public string Owner { get; set; }

        [Required]
        [MaxLength(20)]
        public string BUID { get; set; }

        [Required]
        public bool IsSystem { get; set; }

        [Required]
        public bool Administrator { get; set; }

        [Required]
        public bool SystemAdmin { get; set; }

        [Required]
        public bool FunctionAdmin { get; set; }
        private string _Password;

        [Required]
        [MaxLength(128)]
        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                if (value == null)
                    value = string.Empty;

                _Password = value;
            }
        }


        [MaxLength(128)]
        public string PWQuestion { get; set; }

        [MaxLength(128)]
        public string PWAnswer { get; set; }

        [Required]
        public bool NeverExpire { get; set; }

        [Required]
        public bool FirstChange { get; set; }

        [Required]
        public bool CantChange { get; set; }

        [Required]
        [MaxLength(1)]
        public string SystemFormat { get; set; }

        [Required]
        public bool ActiveTrackLog { get; set; }

        [Required]
        public bool ActiveAuditTrail { get; set; }

        [Required]
        public bool PersonalizeView { get; set; }

        [Required]
        public bool PrintConsolidate { get; set; }

        [MaxLength(1)]
        public string FavouriteView { get; set; }

        [Required]
        public short AutoRefresh { get; set; }

        [Required]
        public short Disconnect { get; set; }

        [MaxLength(500)]
        public string DefaultPath { get; set; }

        [MaxLength(256)]
        public string SessionID { get; set; }

        [MaxLength(5)]
        public string LoginSystem { get; set; }

        [Required]
        public short FailedPWAttempt { get; set; }

        [Required]
        public short FailedAnswerAttempt { get; set; }

        [Required]
        public bool IsOnline { get; set; }

        [Required]
        public bool IsLockedOut { get; set; }

        public DateTime? LastLockout { get; set; }


        public DateTime? LastLogin { get; set; }

        public DateTime? LastPWChange { get; set; }

        [MaxLength(50)]
        public string TrackerID { get; set; }

        public string ExtendInfo { get; set; }

        [Required]
        public decimal Latitude { get; set; }

        [Required]
        public decimal Longitude { get; set; }


        public DateTime? LastUpdated { get; set; }

        public DateTime? Installation { get; set; }

        [MaxLength(300)]
        public string Note { get; set; }

        [Required]
        public bool Stop { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [MaxLength(20)]
        public string CreatedBy { get; set; }


        public DateTime? ModifiedOn { get; set; }

        [MaxLength(20)]
        public string ModifiedBy { get; set; }
        #endregion

        #region Get/Set property value

        public object GetFieldValue(string name)
        {
            return name switch
            {
                "RecID" => RecID,
                "UserID" => UserID,
                "UserType" => UserType,
                "UserName" => UserName,
                "LastName" => LastName,
                "FirstName" => FirstName,
                "SearchName" => SearchName,
                "Description" => Description,
                "DomainUser" => DomainUser,
                "Email" => Email,
                "PersonalEmail" => PersonalEmail,
                "Mobile" => Mobile,
                "Phone" => Phone,
                "Picture" => Picture,
                "SLALevel" => SLALevel,
                "SLA24x7" => SLA24x7,
                "Owner" => Owner,
                "BUID" => BUID,
                "IsSystem" => IsSystem,
                "Administrator" => Administrator,
                "SystemAdmin" => SystemAdmin,
                "FunctionAdmin" => FunctionAdmin,
                "Password" => Password,
                "PWQuestion" => PWQuestion,
                "PWAnswer" => PWAnswer,
                "NeverExpire" => NeverExpire,
                "FirstChange" => FirstChange,
                "CantChange" => CantChange,
                "SystemFormat" => SystemFormat,
                "ActiveTrackLog" => ActiveTrackLog,
                "ActiveAuditTrail" => ActiveAuditTrail,
                "PersonalizeView" => PersonalizeView,
                "PrintConsolidate" => PrintConsolidate,
                "FavouriteView" => FavouriteView,
                "AutoRefresh" => AutoRefresh,
                "Disconnect" => Disconnect,
                "DefaultPath" => DefaultPath,
                "SessionID" => SessionID,
                "LoginSystem" => LoginSystem,
                "FailedPWAttempt" => FailedPWAttempt,
                "FailedAnswerAttempt" => FailedAnswerAttempt,
                "IsOnline" => IsOnline,
                "IsLockedOut" => IsLockedOut,
                "LastLockout" => LastLockout,
                "LastLogin" => LastLogin,
                "LastPWChange" => LastPWChange,
                "TrackerID" => TrackerID,
                "ExtendInfo" => ExtendInfo,
                "Latitude" => Latitude,
                "Longitude" => Longitude,
                "LastUpdated" => LastUpdated,
                "Installation" => Installation,
                "Note" => Note,
                "Stop" => Stop,
                "CreatedOn" => CreatedOn,
                "CreatedBy" => CreatedBy,
                "ModifiedOn" => ModifiedOn,
                "ModifiedBy" => ModifiedBy,
                _ => null
            };
        }

        public void SetFieldValue(string name, dynamic value)
        {
            switch (name)
            {
                case "RecID":
                    RecID = value;
                    break;

                case "UserID":
                    UserID = value;
                    break;

                case "UserType":
                    UserType = value;
                    break;

                case "UserName":
                    UserName = value;
                    break;

                case "LastName":
                    LastName = value;
                    break;

                case "FirstName":
                    FirstName = value;
                    break;

                case "SearchName":
                    SearchName = value;
                    break;

                case "Description":
                    Description = value;
                    break;

                case "DomainUser":
                    DomainUser = value;
                    break;

                case "Email":
                    Email = value;
                    break;

                case "PersonalEmail":
                    PersonalEmail = value;
                    break;

                case "Mobile":
                    Mobile = value;
                    break;

                case "Phone":
                    Phone = value;
                    break;

                case "Picture":
                    Picture = value;
                    break;

                case "SLALevel":
                    SLALevel = value;
                    break;

                case "SLA24x7":
                    SLA24x7 = value;
                    break;

                case "Owner":
                    Owner = value;
                    break;

                case "BUID":
                    BUID = value;
                    break;

                case "IsSystem":
                    IsSystem = value;
                    break;

                case "Administrator":
                    Administrator = value;
                    break;

                case "SystemAdmin":
                    SystemAdmin = value;
                    break;

                case "FunctionAdmin":
                    FunctionAdmin = value;
                    break;

                case "Password":
                    Password = value;
                    break;

                case "PWQuestion":
                    PWQuestion = value;
                    break;

                case "PWAnswer":
                    PWAnswer = value;
                    break;

                case "NeverExpire":
                    NeverExpire = value;
                    break;

                case "FirstChange":
                    FirstChange = value;
                    break;

                case "CantChange":
                    CantChange = value;
                    break;

                case "SystemFormat":
                    SystemFormat = value;
                    break;

                case "ActiveTrackLog":
                    ActiveTrackLog = value;
                    break;

                case "ActiveAuditTrail":
                    ActiveAuditTrail = value;
                    break;

                case "PersonalizeView":
                    PersonalizeView = value;
                    break;

                case "PrintConsolidate":
                    PrintConsolidate = value;
                    break;

                case "FavouriteView":
                    FavouriteView = value;
                    break;

                case "AutoRefresh":
                    AutoRefresh = value;
                    break;

                case "Disconnect":
                    Disconnect = value;
                    break;

                case "DefaultPath":
                    DefaultPath = value;
                    break;

                case "SessionID":
                    SessionID = value;
                    break;

                case "LoginSystem":
                    LoginSystem = value;
                    break;

                case "FailedPWAttempt":
                    FailedPWAttempt = value;
                    break;

                case "FailedAnswerAttempt":
                    FailedAnswerAttempt = value;
                    break;

                case "IsOnline":
                    IsOnline = value;
                    break;

                case "IsLockedOut":
                    IsLockedOut = value;
                    break;

                case "LastLockout":
                    LastLockout = value;
                    break;

                case "LastLogin":
                    LastLogin = value;
                    break;

                case "LastPWChange":
                    LastPWChange = value;
                    break;

                case "TrackerID":
                    TrackerID = value;
                    break;

                case "ExtendInfo":
                    ExtendInfo = value;
                    break;

                case "Latitude":
                    Latitude = value;
                    break;

                case "Longitude":
                    Longitude = value;
                    break;

                case "LastUpdated":
                    LastUpdated = value;
                    break;

                case "Installation":
                    Installation = value;
                    break;

                case "Note":
                    Note = value;
                    break;

                case "Stop":
                    Stop = value;
                    break;

                case "CreatedOn":
                    CreatedOn = value;
                    break;

                case "CreatedBy":
                    CreatedBy = value;
                    break;

                case "ModifiedOn":
                    ModifiedOn = value;
                    break;

                case "ModifiedBy":
                    ModifiedBy = value;
                    break;

            }
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;

using System.Text;

namespace MicroPayment.Model
{
    /// <summary>
    /// 用户基础信息类
    /// </summary>
    public class UserInfo
    {

        public UserInfo()
        {
        }

        private int microUserID;

        /// <summary>
        /// 用户ID
        /// </summary>
        public int MicroUserID
        {
            get { return microUserID; }
            set { microUserID = value; }
        }
        private string microUserName;

        /// <summary>
        /// 用户登陆名
        /// </summary>
        public string MicroUserName
        {
            get { return microUserName; }
            set { microUserName = value; }
        }
        private string microUserRealName;

        /// <summary>
        /// 用户真实姓名
        /// </summary>
        public string MicroUserRealName
        {
            get { return microUserRealName; }
            set { microUserRealName = value; }
        }

        private string microUserPetName;

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string MicroUserPetName
        {
            get { return microUserPetName; }
            set { microUserPetName = value; }
        }
        private string microUserMail;

        /// <summary>
        /// 用户邮箱
        /// </summary>
        public string MicroUserMail
        {
            get { return microUserMail; }
            set { microUserMail = value; }
        }
        private string microUserJuniorPassword;

        /// <summary>
        /// 用户初始密码
        /// </summary>
        public string MicroUserJuniorPassword
        {
            get { return microUserJuniorPassword; }
            set { microUserJuniorPassword = value; }
        }
        private string microSeniorPassword;

        /// <summary>
        /// 用户高级密码
        /// </summary>
        public string MicroSeniorPassword
        {
            get { return microSeniorPassword; }
            set { microSeniorPassword = value; }
        }
        private int microUserAuthority;

        /// <summary>
        /// 用户权限
        /// </summary>
        public int MicroUserAuthority
        {
            get { return microUserAuthority; }
            set { microUserAuthority = value; }
        }
        private DateTime microCreateDate;

        /// <summary>
        /// 用户创建时间
        /// </summary>
        public DateTime MicroCreateDate
        {
            get { return microCreateDate; }
            set { microCreateDate = value; }
        }
        private string microLastLoginIP;
        /// <summary>
        /// 最后登陆IP
        /// </summary>
        public string MicroLastLoginIP
        {
            get { return microLastLoginIP; }
            set { microLastLoginIP = value; }
        }
        private DateTime microLastLoginDate;

        /// <summary>
        /// 最后登陆日期
        /// </summary>
        public DateTime MicroLastLoginDate
        {
            get { return microLastLoginDate; }
            set { microLastLoginDate = value; }
        }
        private string microUserAddress;

        /// <summary>
        /// 用户地址
        /// </summary>
        public string MicroUserAddress
        {
            get { return microUserAddress; }
            set { microUserAddress = value; }
        }
        private string microUserTelephone;

        /// <summary>
        /// 用户联系电话
        /// </summary>
        public string MicroUserTelephone
        {
            get { return microUserTelephone; }
            set { microUserTelephone = value; }
        }
        private string microUserMobile;

        /// <summary>
        /// 用户手机
        /// </summary>
        public string MicroUserMobile
        {
            get { return microUserMobile; }
            set { microUserMobile = value; }
        }
        private int microUserLevel;

        /// <summary>
        /// 用户等级
        /// </summary>
        public int MicroUserLevel
        {
            get { return microUserLevel; }
            set { microUserLevel = value; }
        }
        private int microUserSex;

        /// <summary>
        /// 用户性别
        /// </summary>
        public int MicroUserSex
        {
            get { return microUserSex; }
            set { microUserSex = value; }
        }
        private DateTime microBirthday;

        /// <summary>
        /// 用户生日日期
        /// </summary>
        public DateTime MicroBirthday
        {
            get { return microBirthday; }
            set { microBirthday = value; }
        }
        private string microConstellation;

        /// <summary>
        /// 用户星座
        /// </summary>
        public string MicroConstellation
        {
            get { return microConstellation; }
            set { microConstellation = value; }
        }
        private int isMarry;

        /// <summary>
        /// 用户是否结婚
        /// </summary>
        public int IsMarry
        {
            get { return isMarry; }
            set { isMarry = value; }
        }
        private string country;

        /// <summary>
        /// 用户国家
        /// </summary>
        public string Country
        {
            get { return country; }
            set { country = value; }
        }
        private string province;

        /// <summary>
        /// 用户所属省份
        /// </summary>
        public string Province
        {
            get { return province; }
            set { province = value; }
        }
        private string city;

        /// <summary>
        /// 用户所属城市
        /// </summary>
        public string City
        {
            get { return city; }
            set { city = value; }
        }
    }
}

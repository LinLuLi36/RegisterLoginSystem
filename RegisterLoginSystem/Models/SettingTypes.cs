using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegisterLoginSystem.Models
{
    public class SettingTypes
    {
        public int SettingTypeId { get; set; }
        public string SettingTypeName { get; set; }
        public List<UserSetting> Users { get; set; }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace REDOXEntities.DataBase
{
    using System;
    using System.Collections.Generic;
    
    public partial class FILEBASE
    {
        public int ID { get; set; }
        public string FILE_RANDOM_NM { get; set; }
        public string FILE_REL_NM { get; set; }
        public string FILE_PATH { get; set; }
        public Nullable<int> IDENTIFY_KEY { get; set; }
        public Nullable<double> SQ { get; set; }
        public System.DateTime BUD_DT { get; set; }
        public int BUD_ID { get; set; }
        public int MAP_ID { get; set; }
        public string MAP_SITE { get; set; }
        public string FILE_TP { get; set; }
        public string URL_PATH { get; set; }
    }
}

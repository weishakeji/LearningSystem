using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using WeiSha.Common;
using Song.Entities;

using WeiSha.Data;
using Song.ServiceInterfaces;
using System.Resources;
using System.Reflection;

namespace Song.ServiceImpls
{
    public partial class ContentsCom : IContents
    {
        public int NoteAdd(NewsNote entity)
        {
            entity.Nn_CrtTime = DateTime.Now;
            if (entity.Nn_Details != null)
            {
                if (entity.Nn_Details.Length > 255)
                {
                    entity.Nn_Details = entity.Nn_Details.Substring(0, 255);
                }
            }
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null)
            {
                entity.Org_ID = org.Org_ID;
                entity.Org_Name = org.Org_Name;
            }
            return Gateway.Default.Save<NewsNote>(entity);
        }

        public void NoteSave(NewsNote entity)
        {
            Gateway.Default.Save<NewsNote>(entity);
        }

        public void NoteDelete(int identify)
        {
            Gateway.Default.Delete<NewsNote>(NewsNote._.Nn_Id == identify);
        }

        public NewsNote NoteSingle(int identify)
        {
            return Gateway.Default.From<NewsNote>().Where(NewsNote._.Nn_Id == identify).ToFirst<NewsNote>();
        }

        public NewsNote[] NoteCount(int artid, bool? isShow, int count)
        {
            WhereClip wc = NewsNote._.Art_Id == artid;
            if (isShow != null) wc.And(NewsNote._.Nn_IsShow == (bool)isShow);
            if (count < 1)
            {
                return Gateway.Default.From<NewsNote>().Where(wc).OrderBy(NewsNote._.Nn_CrtTime.Desc).ToArray<NewsNote>();
            }
            return Gateway.Default.From<NewsNote>().Where(wc).OrderBy(NewsNote._.Nn_CrtTime.Desc).ToArray<NewsNote>(count);
        }

        public NewsNote[] NotePager(int artid, string searTxt, bool? isShow, int size, int index, out int countSum)
        {
            WhereClip wc = NewsNote._.Art_Id == artid;
            if (isShow != null) wc.And(NewsNote._.Nn_IsShow == (bool)isShow);
            if (searTxt != null && searTxt.Length > 0)
            {
                wc.And(NewsNote._.Nn_Title.Like("%" + searTxt + "%"));
            }
            countSum = Gateway.Default.Count<NewsNote>(wc);
            return Gateway.Default.From<NewsNote>().Where(wc).OrderBy(NewsNote._.Nn_CrtTime.Desc).ToArray<NewsNote>(size, (index - 1) * size);
        }

    }
}

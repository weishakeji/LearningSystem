using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using WeiSha.Common;
using Song.Entities;

using WeiSha.Data;
using Song.ServiceInterfaces;
using System.Data.Common;



namespace Song.ServiceImpls
{
    public class MobileUserCom : IMobileUser
    {

        public int Add(MobileUser entity)
        {
            entity.Mu_LastTime = DateTime.Now;
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null)
            {
                entity.Org_ID = org.Org_ID;
                entity.Org_Name = org.Org_Name;
            }
            return Gateway.Default.Save<MobileUser>(entity);
        }

        public void Save(MobileUser entity)
        {
            entity.Mu_LastTime = DateTime.Now;
            Gateway.Default.Save<MobileUser>(entity);
        }

        public void Delete(MobileUser entity)
        {
            Gateway.Default.Delete<MobileUser>(entity);
        }

        public void Delete(int identify)
        {
            Gateway.Default.Delete<MobileUser>(MobileUser._.Mu_Id == identify);
        }

        public MobileUser GetSingle(int identify)
        {
            return Gateway.Default.From<MobileUser>().Where(MobileUser._.Mu_Id == identify).ToFirst<MobileUser>();
        }
        public MobileUser GetSingle(string phone)
        {
            return Gateway.Default.From<MobileUser>().Where(MobileUser._.Mu_Phone== phone.Trim()).ToFirst<MobileUser>();
        }

        public int GetCount()
        {
            return Gateway.Default.Count<MobileUser>(MobileUser._.Mu_Id != -1);
        }

        public MobileUser[] GetPager(int size, int index, out int countSum)
        {
            WhereClip wc = MobileUser._.Mu_Id != -1;
            countSum = Gateway.Default.Count<MobileUser>(wc);
            Song.Entities.MobileUser[] ac = Gateway.Default.From<MobileUser>().Where(wc).OrderBy(MobileUser._.Mu_LastTime.Desc).ToArray<MobileUser>(size, (index - 1) * size);
            return ac;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Entities;
using VTemplate.Engine;

namespace Song.Template.Tags.ListTag
{
    /// <summary>
    /// 教师列表
    /// </summary>
    public class Teacher : TagElement
    {
        public override void DataBind()
        {
            VTemplate.Engine.ListTag tag = this.Tag as VTemplate.Engine.ListTag;
            if (tag == null) return;
            //
            int thCount = int.Parse(this.Tag.Attributes.GetValue("count", "10"));
            Song.Entities.Teacher[] teachers = Business.Do<ITeacher>().TeacherCount(Organ.Org_ID, true, thCount);
            foreach (Song.Entities.Teacher t in teachers)
            {
                Song.Entities.Accounts acc = Business.Do<IAccounts>().AccountsSingle(t.Ac_ID);
                //教师照片
                if (string.IsNullOrWhiteSpace(t.Th_Photo))
                {
                    if (acc != null)
                    {
                        t.Th_Photo = acc.Ac_Photo;
                        Business.Do<ITeacher>().TeacherSave(t);
                    }
                }
                if (!string.IsNullOrWhiteSpace(t.Th_Photo))
                {
                    t.Th_Photo = Upload.Get["Teacher"].Virtual + t.Th_Photo;
                }
            }
            tag.DataSourse = teachers;    
        }
    }
}

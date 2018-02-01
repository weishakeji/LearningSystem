using System;
using System.Configuration;
using System.Text.RegularExpressions;
namespace Song.Extend
{
    public class VideoMethod : System.Web.UI.Page
    {
        public VideoMethod()
        {

        }
        //文件路径
        public static string ffmpegtool = WeiSha.Common.App.Get["ffmpeg"].String;
        public static string mencodertool = WeiSha.Common.App.Get["mencoder"].String;
        //文件图片大小
        public static string sizeOfImg = ConfigurationManager.AppSettings["CatchFlvImgSize"];
        //文件大小
        public static string widthOfFile = WeiSha.Common.App.Get["widthSize"].String;
        public static string heightOfFile = WeiSha.Common.App.Get["heightSize"].String;
        //获取文件的名字
        public static string GetFileName(string fileName)
        {
            int i = fileName.LastIndexOf("\\") + 1;
            string Name = fileName.Substring(i);
            return Name;
        }
        //获取文件扩展名
        public static string GetExtension(string fileName)
        {
            int i = fileName.LastIndexOf(".") + 1;
            string Name = fileName.Substring(i);
            return Name;
        }
        //
        #region //运行FFMpeg的视频解码，(这里是绝对路径)
        /// <summary>
        /// 转换文件并保存在指定文件夹下面(这里是绝对路径)
        /// </summary>
        /// <param name="fileName">上传视频文件的路径（原文件）</param>
        /// <param name="playFile">转换后的文件的路径（网络播放文件）</param>
        /// <param name="imgFile">从视频文件中抓取的图片路径</param>
        /// <returns>成功:返回图片虚拟地址;   失败:返回空字符串</returns>
        public string FFfor_Flv(string fileName, string playFile, string imgFile)
        {
            //取得ffmpeg.exe的路径,路径配置在Web.Config中,如:<add   key="ffmpeg"   value="E:\aspx1\ffmpeg.exe"   />   
            string ffmpeg = Server.MapPath(VideoMethod.ffmpegtool);
            fileName = Server.MapPath(fileName);
            if ((!System.IO.File.Exists(ffmpeg)) || (!System.IO.File.Exists(fileName))) return "";          

            //获得图片和(.flv)文件相对路径/最后存储到数据库的路径,如:/Web/User1/00001.jpg   
            string videofile = Server.MapPath(System.IO.Path.ChangeExtension(playFile, ".flv"));
            //截图的尺寸大小,配置在Web.Config中,如:<add   key="CatchFlvImgSize"   value="240x180"   />   
            string FlvImgSize = VideoMethod.sizeOfImg;
            System.Diagnostics.ProcessStartInfo FilestartInfo = new System.Diagnostics.ProcessStartInfo(ffmpeg);
            //FilestartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            //◾-i filename：指定输入文件名。
            //◾-ab bitrate：设定音频编码器输出的比特率，整数，单位bps。
            //◾-ar sample rate：设定音频编解码器的采样率，整数，单位Hz。
            // -qscale q 使用固定的视频量化标度(VBR)
            //◾-b bitrate：设定音视频编码器输出的比特率，整数，单位bps，缺省200kb/s
            // -r fps：设定视频编解码器的帧率，整数，单位fps 缺省25
            // -s size 设置帧大小 格式为WXH 缺省160X128.下面的简写也可以直接使用：
            //FilestartInfo.Arguments = " -i " + fileName + " -ab 56 -ar 22050 -qscale 6 -r 15 -s " + widthOfFile + "x" + heightOfFile + " -y " + videofile;
            //FilestartInfo.Arguments = " -i " + fileName + " -y  -ab 56 -ar 22050 -b 1500 -r 29.97 -s 1280*720 -qscale 0.01 " + videofile;  //mp4
            FilestartInfo.Arguments = " -i " + fileName + " -f flv -ab 56 -ar 22050 -qscale " + 3 + " -r 15 -s " + widthOfFile + "x" + heightOfFile + " -y " + videofile; ;  //fkv
            try
            {
                if (!string.IsNullOrWhiteSpace(imgFile))
                {
                    //截图
                    CatchImg(fileName, imgFile);
                }
                //转换
                FilestartInfo.UseShellExecute = false;
                //FilestartInfo.CreateNoWindow = true;
                System.Diagnostics.Process.Start(FilestartInfo);

            }
            catch
            {
                return "";
            }
            //
            return "";
        }
        //
        public string CatchImg(string fileName, string imgFile)
        {
            string ffmpeg = Server.MapPath(VideoMethod.ffmpegtool);
            string flv_img = imgFile + ".jpg";
            string FlvImgSize = VideoMethod.sizeOfImg;
            System.Diagnostics.ProcessStartInfo ImgstartInfo = new System.Diagnostics.ProcessStartInfo(ffmpeg);
            ImgstartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            //
            ImgstartInfo.Arguments = "   -i   " + fileName + "  -y  -f  image2   -ss 2 -vframes 1  -s   " + FlvImgSize + "   " + flv_img;
            try
            {
                System.Diagnostics.Process.Start(ImgstartInfo);
            }
            catch
            {
                return "";
            }
            //
            if (System.IO.File.Exists(flv_img))
            {
                return flv_img;
            }

            return "";
        }
        #endregion
 

        #region //运行mencoder的视频解码器转换(这里是(绝对路径))
        public string MCfor_Flv(string vFileName, string playFile, string imgFile)
        {
            string tool = Server.MapPath(VideoMethod.mencodertool);
            //string mplaytool = Server.MapPath(VideoMethod.ffmpegtool);

            if ((!System.IO.File.Exists(tool)) || (!System.IO.File.Exists(vFileName))) return ""; 
            string flv_file = System.IO.Path.ChangeExtension(playFile, ".flv");

            //截图的尺寸大小,配置在Web.Config中,如:<add   key="CatchFlvImgSize"   value="240x180"   />   
            string FlvImgSize = VideoMethod.sizeOfImg;
            System.Diagnostics.ProcessStartInfo FilestartInfo = new System.Diagnostics.ProcessStartInfo(tool);
            FilestartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            FilestartInfo.Arguments = "  -quiet   -oac mp3lame -lameopts abr:br=56 -srate 22050 -af channels=2       -ovc lavc   -vf harddup,hqdn3d,scale=176:-3   -lavcopts vcodec=flv:vbitrate=152:mbd=2:trell:v4mv:turbo:keyint=45 -ofps 15 -of lavf   " + vFileName + " -o " + flv_file;
            //FilestartInfo.Arguments = " " + vFileName + " -o " + flv_file + " -of lavf -lavfopts i_certify_that_my_video_stream_does_not_use_b_frames -oac mp3lame -lameopts abr:br=56 -ovc lavc -lavcopts vcodec=flv:vbitrate=200:mbd=2:mv0:trell:v4mv:cbp:last_pred=1:dia=-1:cmp=0:vb_strategy=1 -vf scale=" + widthOfFile + ":" + heightOfFile + " -ofps 12 -srate 22050"; 
            try
            {
                FilestartInfo.UseShellExecute = false;
                System.Diagnostics.Process.Start(FilestartInfo);
                //CatchImg(flv_file, imgFile);
            }
            catch
            {
                return "";
            }
            //
            return "";
        }
        #endregion

        #region 获取视频类型
        // 扩展名定义
        string[] strArrFfmpeg = new string[] { "asf", "avi", "mpg", "3gp", "mov", "mp4", "mkv" };
        string[] strArrMencoder = new string[] { "wmv", "rmvb", "rm" };
        /// <summary>
        /// 判断视频类型
        /// </summary>
        /// <param name="extension"></param>
        /// <returns>返回ffmpeg，或 mencoder</returns>
        public string CheckExtension(string extension)
        {
            extension = extension.Replace(".", "");
            string m_strReturn = "";
            foreach (string var in this.strArrFfmpeg)
            {
                if (var == extension)
                {
                    m_strReturn = "ffmpeg"; break;
                }
            }
            if (m_strReturn == "")
            {
                foreach (string var in strArrMencoder)
                {
                    if (var == extension)
                    {
                        m_strReturn = "mencoder"; break;
                    }
                }
            }
            return m_strReturn;
        }
        #endregion
    }
}

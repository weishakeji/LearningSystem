/*
文件上传总进度
*/
/*
声明全局变量
*/
//全部上传文件总大小
var fg_fileSizes = 0;

//已上传文件大小
var fg_uploads = 0;

//总上传进度对象
var fg_object;
//Download by http://www.codefans.net
function FileGroupProgress()
{
    this.fileGroupElement = document.getElementById(swfu.settings.custom_settings.s_cnt_progress);
    if (!this.fileGroupElement)
    {
        this.fileGroupElement = document.createElement("div");
        this.fileGroupElement.id = swfu.settings.custom_settings.s_cnt_progress;
        this.fileGroupElement.className = swfu.settings.custom_settings.container_css;

        //上传总进度文本0
        this.fileSumSpan = document.createElement("span");
        this.fileSumSpan.className = swfu.settings.custom_settings.s_cnt_span_text;
        this.fileSumSpan.appendChild(document.createTextNode("上传总进度："));
        this.fileGroupElement.appendChild(this.fileSumSpan);

        //进度条容器1
        this.fileProgressDiv = document.createElement("div");
        this.fileProgressDiv.className = "statebarBigDiv";

        //进度条1.1
        this.fileStateBar = document.createElement("div");
        this.fileStateBar.className = "statebar";
        this.fileStateBar.id = swfu.settings.custom_settings.s_cnt_progress_statebar;
        this.fileStateBar.innerHTML = "&nbsp;";
        this.fileStateBar.style.width = "0%";
        this.fileProgressDiv.appendChild(this.fileStateBar);
        this.fileGroupElement.appendChild(this.fileProgressDiv);

        //百分比2
        this.filePercent = document.createElement("span");
        this.filePercent.className = "ftt";
        this.filePercent.id = swfu.settings.custom_settings.s_cnt_progress_percent;
        this.fileGroupElement.appendChild(this.filePercent);

        //已上传文本3
        this.spanTextU = document.createElement("span");
        this.spanTextU.appendChild(document.createTextNode("，已上传"));
        this.fileGroupElement.appendChild(this.spanTextU);

        //已上传大小4
        this.fileUploadSize = document.createElement("span");
        this.fileUploadSize.id = swfu.settings.custom_settings.s_cnt_progress_uploaded;
        this.fileUploadSize.innerHTML = "0bytes";
        this.fileGroupElement.appendChild(this.fileUploadSize);

        //普通文本5
        this.spanText = document.createElement("span");
        this.spanText.appendChild(document.createTextNode("，总文件大小"));
        this.fileGroupElement.appendChild(this.spanText);

        //上传总量6
        this.fileUploadCount = document.createElement("span");
        this.fileUploadCount.id = swfu.settings.custom_settings.s_cnt_progress_size;
        this.fileGroupElement.appendChild(this.fileUploadCount);

        document.getElementById(swfu.settings.custom_settings.progressGroupTarget).appendChild(this.fileGroupElement);
    }
    else
    {
        this.fileStateBar = this.fileGroupElement.childNodes[1].childNodes[0];
        this.filePercent = this.fileGroupElement.childNodes[2];
        this.fileUploadSize = this.fileGroupElement.childNodes[4];
        this.fileUploadCount = this.fileGroupElement.childNodes[6];
    }
}

//设置全部上传文件大小
FileGroupProgress.prototype.setFileCountSize = function(filesize)
{
    this.fileUploadCount.innerHTML = formatUnits(filesize);
    //如果没有上传文件,则隐藏控件
    if (filesize == 0)
    {
        this.show(false);
    }
}

/*
设置上传进度

*/
FileGroupProgress.prototype.setUploadProgress = function(uploads, filesize)
{
    if (uploads == 0 | filesize == 0) return;
    var percent = Math.ceil((uploads / filesize) * 100);
    this.fileStateBar.style.width = percent + "%";
    this.filePercent.innerHTML = percent + "%";

    this.fileUploadSize.innerHTML = formatUnits(uploads);
}

//设置控件显示状态
FileGroupProgress.prototype.show = function(show)
{
    this.fileGroupElement.style.display = show ? "" : "none";
}

//计算文件大小的文字描述,传入参数单位为字节
function formatUnits(size)
{    
    if (isNaN(size) || size == null)
    {
        size = 0;
    }

    if (size <= 0) return size + "bytes";

    var t1 = (size / 1024).toFixed(2);
    if (t1 < 0)
    {
        return "0KB";
    }

    if (t1 > 0 && t1 < 1024)
    {
        return t1 + "KB";
    }

    var t2 = (t1 / 1024).toFixed(2);
    if (t2 < 1024)
        return t2 + "MB";

    return (t2 / 1024).toFixed(2) + "GB";
}
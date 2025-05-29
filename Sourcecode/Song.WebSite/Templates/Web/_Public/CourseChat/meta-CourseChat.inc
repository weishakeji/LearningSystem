<meta charset="utf-8">
<meta name="viewport" content="initial-scale=1,maximum-scale=1,user-scalable=no" />
<meta name="viewport" content="width=device-width, initial-scale=1,maximum-scale=1.0, user-scalable=no" />
<meta name="apple-mobile-web-app-capable" content="yes" />
<meta name="apple-mobile-web-app-status-bar-style" content="black" />
<meta name="format-detection" content="telephone=yes" />
<meta name="format-detection" content="email=no" />
<script type="text/javascript" src="Scripts/webdom.js"></script>
<script type="text/javascript" src="Scripts/Public.js"></script>
<style type="text/css">
    *[v-cloak]{
            text-align:left !important;
            color:#999;   
        }
        *[v-cloak]>* {
             display: none !important;
        }
        *[v-cloak]::before {
            content: "/";
            display: block;
            width:24px;
            height:24px;
            text-align: center;
            float:left;
            font-size: 16px;           
            animation: degree_rotation_360 0.5s infinite linear;
            transform-origin: center center;
        }
        *[v-cloak]::after {
            content: "loading ... ";
            display: block;     
            font-size: 13px;
            margin-left:10px;
            line-height:20px;               
            position: absolute;
            top:8px;
            left: 20px;
        }
    @keyframes degree_rotation_360 {
       from { transform: rotate(0deg); }
      to { transform: rotate(360deg); }
    }

</style>
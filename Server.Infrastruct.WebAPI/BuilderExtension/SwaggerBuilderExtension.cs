using Common.Toolkit.Helper;

namespace Server.Infrastruct.WebAPI.BuilderExtension
{
    public static class SwaggerBuilderExtension
    {
        /// <summary>
        /// Swagger注册
        /// </summary>
        /// <param name="app"></param>
        /// <param name="swaggerURL">swagger访问路径</param>
        public static void UseCustomSwagger(this IApplicationBuilder app, string swaggerURL = "swagger")
        {
            string apiName = AppSettingsHelper.GetSetting("SwaggerSetting", "ApiName");
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "{documentName}/swagger.json";
            });

            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = swaggerURL;
                c.SwaggerEndpoint($"/{apiName}/swagger.json", apiName);
                //c.InjectJavascript
                c.HeadContent = @"<script  type='text/javascript'>

                        function getUrl(btn){

                        let text=btn.innerText;

                    	let startIndex=text.indexOf('\n');
                    	let endIndex=text.lastIndexOf('\n');
                    	//let url=text.substr(startIndex+1,endIndex-startIndex-1);
                    	let url=text.substr(0,endIndex);
                        let encodeURL=encodeURI(url);
                        let index=encodeURL.indexOf('/');
                        url=encodeURL.substr(index,encodeURL.length-index);
                        url=decodeURI(url);


                        let result='';
                        for(let i=0;i<url.length;i++){
                            //let value=parseInt(url[i]);

                            let value = url[i].charCodeAt();
                                        if (value<=31 || value>128){
                                            //console.log('error char ');
                                        }
                                        else{
                                            result=result+url[i];
                                        }
                        }
                        url=result;


                    	//alert( url);
                    	console.log(url);
                        navigator.clipboard.writeText(url);
                        //navigator.clipboard.writeText('test');
                    }

                    //let load=window.onload;
                    function init(){
                        if(document.location.protocol==='http:'){
                            return;
                        }
                    	let btns=document.getElementsByClassName('opblock-summary-path');
                    	//let btns=document.getElementsByClassName('opblock-summary-control');
                    	//let btns=document.getElementsByClassName('opblock-summary opblock-summary-get');
                        console.log(btns.length);
                    	for(let i=0;i<btns.length;i++){
                    		//let btn=btns[i];

                    		let btn=document.createElement('button');
                    		btn.textContent='获取url';
                    		//btn.click=getUrl(btns[i]);
                    		btn.addEventListener('click', function(e){getUrl(btns[i]); e.stopPropagation(); });
                    		btns[i].append(btn);
                    	}
                    }
                    console.log('init');
                    //window.addEventListener('load',init,false);
                    window.setTimeout(init,5000);
                    
                    </script>";
            });
        }
    }
}

﻿

#pragma checksum "D:\个人资料\William\工程\WeiXinAssistant\WeiXinAssistant\TalkPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "6B9884F4B2D80D8A46567B58747197C8"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WeiXinAssistant
{
    partial class TalkPage : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 28 "..\..\..\TalkPage.xaml"
                ((global::Windows.UI.Xaml.Controls.ListViewBase)(target)).ItemClick += this.inView_ItemClick;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 44 "..\..\..\TalkPage.xaml"
                ((global::Windows.UI.Xaml.FrameworkElement)(target)).Loaded += this.Item_Loaded;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}



﻿

#pragma checksum "D:\个人资料\William\工程\WeiXinAssistant\WeiXinAssistant\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "D8B7DBA6A7C9EB3D09E868887CD6CDB5"
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
    partial class MainPage : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 36 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.HyperlinkButton_Click;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 37 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.HyperlinkButton1_Click;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 24 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.FrameworkElement)(target)).Loaded += this.Account_Loaded;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 25 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).Tapped += this.Password_Tapped;
                 #line default
                 #line hidden
                #line 25 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).GotFocus += this.Password_GotFocus;
                 #line default
                 #line hidden
                #line 25 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.FrameworkElement)(target)).Loaded += this.Password_Loaded;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 26 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.FrameworkElement)(target)).Loaded += this.VeriCode_Loaded;
                 #line default
                 #line hidden
                break;
            case 6:
                #line 29 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Button_Click;
                 #line default
                 #line hidden
                #line 29 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.FrameworkElement)(target)).Loaded += this.LoginButton_Loaded;
                 #line default
                 #line hidden
                break;
            case 7:
                #line 30 "..\..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).Tapped += this.VeriCodeImage_Tapped;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}



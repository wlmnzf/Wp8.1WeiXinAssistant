﻿

#pragma checksum "D:\个人资料\William\工程\WeiXinAssistant\WeiXinAssistant\SendGroup.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "D0CE1EF6F7839820A3E89B7CB615D257"
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
    partial class SendGroup : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 12 "..\..\..\SendGroup.xaml"
                ((global::Windows.UI.Xaml.Controls.TextBox)(target)).TextChanged += this.SendBox_TextChanged;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 13 "..\..\..\SendGroup.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Send_Click;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 14 "..\..\..\SendGroup.xaml"
                ((global::Windows.UI.Xaml.Controls.TextBlock)(target)).SelectionChanged += this.textCount_SelectionChanged;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}



﻿#pragma checksum "..\..\UserDetails.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "2953B8B59782C46C2B454B6080D8BD44CD7455BE"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using GUI;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace GUI {
    
    
    /// <summary>
    /// UserDetails
    /// </summary>
    public partial class UserDetails : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 9 "..\..\UserDetails.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid OuterGrid;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\UserDetails.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid InnerGrid;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\UserDetails.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock Name;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\UserDetails.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock Surname;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\UserDetails.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock Role;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\UserDetails.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock Serial;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\UserDetails.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock CurrentPosition;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\UserDetails.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button History;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\UserDetails.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button modify;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/GUI;component/userdetails.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\UserDetails.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.OuterGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.InnerGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.Name = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.Surname = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.Role = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.Serial = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 7:
            this.CurrentPosition = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 8:
            this.History = ((System.Windows.Controls.Button)(target));
            
            #line 41 "..\..\UserDetails.xaml"
            this.History.Click += new System.Windows.RoutedEventHandler(this.History_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.modify = ((System.Windows.Controls.Button)(target));
            
            #line 42 "..\..\UserDetails.xaml"
            this.modify.Click += new System.Windows.RoutedEventHandler(this.modify_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}


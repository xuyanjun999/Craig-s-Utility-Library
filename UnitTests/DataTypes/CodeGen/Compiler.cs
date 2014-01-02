﻿/*
Copyright (c) 2014 <a href="http://www.gutgames.com">James Craig</a>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.*/

using System;
using System.Linq;
using Utilities.DataTypes.CodeGen.BaseClasses;
using Utilities.IO;
using Utilities.IO.FileSystem.Interfaces;
using Xunit;
using Utilities.DataTypes;

namespace UnitTests.DataTypes.CodeGen
{
    public class Compiler
    {
        [Fact]
        public void Creation()
        {
            string File = "";
            using (Utilities.DataTypes.CodeGen.Compiler Test = new Utilities.DataTypes.CodeGen.Compiler("Somewhere", typeof(CompilerBase).Assembly.Location.Left(typeof(CompilerBase).Assembly.Location.LastIndexOf('\\')), true))
            {
                Assert.Equal(0, Test.Classes.Count);
                Assert.Equal(typeof(CompilerBase).Assembly.Location.Left(typeof(CompilerBase).Assembly.Location.LastIndexOf('\\')), Test.AssemblyDirectory);
                Assert.Equal("Somewhere", Test.AssemblyName);
                File = Test.AssemblyDirectory + "/" + Test.AssemblyName + ".dll";
            }
            Assert.True(new Utilities.IO.FileInfo(File).Exists);
            new Utilities.IO.FileInfo(File).Delete();
        }

        [Fact]
        public void CreateType()
        {
            string File = "";
            using (Utilities.DataTypes.CodeGen.Compiler Test = new Utilities.DataTypes.CodeGen.Compiler("Test", ".", true))
            {
                Type Object = Test.CreateClass("A", "public class A{ public string Value1{get;set;}}", null, typeof(object).Assembly);
                Assert.Equal("Test, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", Object.Assembly.FullName);
                Assert.Equal("A", Object.FullName);
                File = Test.AssemblyDirectory + "/" + Test.AssemblyName + ".dll";
            }
            new Utilities.IO.FileInfo(File).Delete();
        }
    }
}
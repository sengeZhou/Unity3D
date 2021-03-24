﻿/****************************************************************************
 * Copyright (c) 2017 snowcold
 * Copyright (c) 2017 ~ 2019.1 liangxie
 * 
 * http://qframework.io
 * https://github.com/liangxiegame/QFramework
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 ****************************************************************************/

using System.Collections.Generic;
using System.Linq;

namespace QFramework
{
    public static class ResFactory
    {
        public static IRes Create(ResSearchKeys resSearchKeys)
        {
            /*
                new ResourcesResCreator(),
                new AssetBundleResCreator(),
                new AssetResCreator(),
                new AssetBundleSceneResCreator(),
                new NetImageResCreator(),
                new LocalImageResCreator()
             */
            var retRes = mResCreators
                .Where(creator => creator.Match(resSearchKeys))  //根据配备规则找到对应的createtor
                .Select(creator => creator.Create(resSearchKeys)) //用creator的create方法创建一个IRes对象
                .FirstOrDefault();

            if (retRes == null)
            {
                Log.E("Failed to Create Res. Not Find By ResSearchKeys:" + resSearchKeys);
                return null;
            }

            return retRes;
        }

        public static void AddResCreator<T>() where T : IResCreator, new()
        {
            mResCreators.Add(new T());
        }

        public static void RemoveResCreator<T>() where T : IResCreator, new()
        {
            mResCreators.RemoveAll(r => r.GetType() == typeof(T));
        }

        public static void AddResCreator(IResCreator resCreator)
        {
            mResCreators.Add(resCreator);
        }

        static List<IResCreator> mResCreators = new List<IResCreator>()
        {
            new ResourcesResCreator(),
            new AssetBundleResCreator(),
            new AssetResCreator(),
            new AssetBundleSceneResCreator(),
            new NetImageResCreator(),
            new LocalImageResCreator()
        };
    }

    public class NetImageResCreator : IResCreator
    {
        public bool Match(ResSearchKeys resSearchKeys)
        {
            return resSearchKeys.AssetName.StartsWith("netimage:");
        }

        public IRes Create(ResSearchKeys resSearchKeys)
        {
            return NetImageRes.Allocate(resSearchKeys.AssetName);
        }
    }

    public class LocalImageResCreator : IResCreator
    {
        public bool Match(ResSearchKeys resSearchKeys)
        {
            return resSearchKeys.AssetName.StartsWith("localimage:");
        }

        public IRes Create(ResSearchKeys resSearchKeys)
        {
            return NetImageRes.Allocate(resSearchKeys.AssetName);
        }
    }
}
# SMLite

[![license](https://img.shields.io/github/license/fawdlstty/SMLite?color=09f)](./LICENSE)
[![cpp](https://img.shields.io/lgtm/grade/cpp/github/fawdlstty/SMLite)](https://lgtm.com/projects/g/fawdlstty/SMLite)
[![nuget](https://img.shields.io/nuget/dt/Fawdlstty.SMLite?label=nuget%20downloads)](https://www.nuget.org/packages/Fawdlstty.SMLite)
[![Total alerts](https://img.shields.io/lgtm/alerts/g/fawdlstty/SMLite.svg?logo=lgtm)](https://lgtm.com/projects/g/fawdlstty/SMLite/alerts/)

<!--
[![csharp](https://img.shields.io/lgtm/grade/csharp/github/fawdlstty/SMLite)](https://lgtm.com/projects/g/fawdlstty/SMLite)
[![python](https://img.shields.io/lgtm/grade/python/github/fawdlstty/SMLite)](https://lgtm.com/projects/g/fawdlstty/SMLite)
[![AppVeyor Build](https://img.shields.io/appveyor/build/fawdlstty/SMLite)](https://ci.appveyor.com/project/fawdlstty/SMLite)
[![Coverage Status](https://coveralls.io/repos/github/fawdlstty/SMLite/badge.svg)](https://coveralls.io/github/fawdlstty/SMLite)
-->

[English](./README.md) | 简体中文

C、C++、C#、Java、JavaScript、Python、VB.Net 的状态机库

## 支持的环境

- C库 支持的版本
    + `C99` 及以上
- C++库 支持的版本
    + `C++ 11` 及以上
- C#库（VB.Net） 支持的运行时
    + `.Net Framework 4.5` 及以上
    + `.Net Standard 2.0/2.1`
    + `.Net 5`
- Java库 支持的版本
    + Java 8
- JavaScript库 支持的版本
    + ECMAScript 2015
- Python库 支持的运行时
    + `Python 3.7`

## 实现进度

|           |   C   |  C++  | C#/VB.Net |  Java  | JavaScript | Python |
|   :---:   | :---: | :---: |   :---:   | :---: |    :---:    | :---: |
|    文档    |   √   |   √   |     √     |       |             |   √   |
| 同步状态机 |   √   |   √   |     √     |   √   |      √      |   √   |
| 多状态支持 |   ×   |   √   |     √     |   √   |             |       |
|  线程安全  |       |   √   |     √     |   √   |             |       |
| 异步状态机 |   ×   |       |     √     |   ×   |      √      |   √   |
|  异步撤销  |   ×   |   ×   |     √     |   ×   |             |       |
|   序列化   |       |   √   |     √     |       |             |       |
|    测试    |   √   |   √   |     √     |   √   |      √      |   √   |

## 用户手册

中国大陆用户加速访问：[文档镜像](https://www.fawdlstty.com/smlite/)

- [C库用户手册](docs/chapters/c_zh.md)
- [C++库用户手册](docs/chapters/cpp_zh.md)
- [C#库用户手册](docs/chapters/csharp_zh.md)
- Java库用户手册
- JavaScript库用户手册
- [Python库用户手册](docs/chapters/python_zh.md)
- [VB.Net库用户手册](docs/chapters/vb.net_zh.md)

## 依赖库

项目有使用到子模块，如果需要本地编译那么需初始化子模块。

```cmd
git clone https://github.com/fawdlstty/SMLite
git submodule update --init --recursive
```

- tstl2cl
    + https://sourceforge.net/projects/tstl2cl/
    + 开源协议: zlib/libpng License

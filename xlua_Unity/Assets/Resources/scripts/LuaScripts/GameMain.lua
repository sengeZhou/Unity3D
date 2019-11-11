
require("LuaDebug")("localhost", 7003)

local profiler = require("perf.profiler")  -- 考虑后面写成

profiler.start()

print("直接引用.lua文件===========GameMain.lua")

require "CSharpCallxLua.CallLuaByGlableVar"  -- 这里面的require也是使用自定义的加载函数
require "CSharpCallxLua.CallLuaFunctionByLuaFun"
require "CSharpCallxLua.CallLuaTable"
require "xluaCallCSharp.xluaCallCSharp"


local CustomHelper = CS.Hxp.CustomHelper

-- 静态方法 可以参考导出的类结构一般使用.
local sum = CustomHelper.Add(2,3)
print("CustomHelper sum======",sum)

-- 对象方法 一般使用:
local customH = CustomHelper()
local str = customH:HString("age")
print("CustomHelper str======", str)

-- 调用时长分析工具
local Application = CS.UnityEngine.Application
local outfile = io.open(Application.dataPath .."/profile.log", "w+")
outfile:write(profiler.report())
outfile:close()
profiler.stop()


-- 内存泄漏分析
-- local memory = require("perf.memory")
-- print("total memory:", memory.total())
-- outfile = io.open(Application.dataPath .. "/memory.log", "w+")
-- outfile:write(memory.snapshot())
-- outfile:close()



-- 给UGUI添加事件 event trigger方式
local GameObject = CS.UnityEngine.GameObject
local  control = GameObject.Find("CustomImage")

control:AddComponent(typeof(CS.UnityEngine.EventSystems.EventTrigger))

--[===[

 EventTrigger eventTrigger = control.AddComponent();
        //新建事件
        EventTrigger.Entry entry = new EventTrigger.Entry();
        //UI事件类型
        entry.eventID = EventTriggerType.PointerClick;
        //添加响应函数
        entry.callback.AddListener(EventTriggerClick);
        //将事件对象压入eventTrigger的triggers中
        eventTrigger.triggers.Add(entry);
]===]







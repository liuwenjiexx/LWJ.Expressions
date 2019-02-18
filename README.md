# LWJ.Expressions
xml script language

add 方法
``` C#
 int add(int a,int b){
   return a+b;
 }
```
```
<x:func name="add" returnType="int">
  <x:vars>
    <x:var type="int">a</x:var>
    <x:var type="int">b</x:var>
  </x:vars>
  <x:return>
    <x:add>
      <x:var>a</x:var>
      <x:var>b</x:var>
    </x:add>
  </x:return>
</x:func>
```

递归调用
``` C#
static int RecursiveSum(int n)
{
    return n <= 1 ? 1 : n + RecursiveSum(--n);
}
```
```
  <x:func name="sum" returnType="int">
    <x:vars>
      <x:var type="int">n</x:var>
    </x:vars>
    <x:return>
      <x:if>
        <x:leq>
          <x:var>n</x:var>
          <x:int>1</x:int>
        </x:leq>
        <x:int>1</x:int>        
        <x:else>
          <x:add>
            <x:var>n</x:var>
            <x:call>
              <x:var>sum</x:var>
              <x:postDecrement>
                <x:var>n</x:var>
              </x:postDecrement>
            </x:call>
          </x:add>
        </x:else>
      </x:if>
    </x:return>
  </x:func>
```


## Project reference:
* [Crystal Defense](https://play.google.com/store/apps/details?id=com.lwj.crystaldefense) skill module

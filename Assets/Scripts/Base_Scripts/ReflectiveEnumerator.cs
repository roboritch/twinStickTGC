using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class ReflectiveEnumerator {
	static ReflectiveEnumerator() { }

	/// <summary>
	/// returns an enumerable list of constructed cards 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="constructorArgs"></param>
	/// <returns></returns>
	public static IEnumerable<T> GetEnumerableOfType<T>(params object[] constructorArgs) where T : class {
		List<T> objects = new List<T>();
		foreach(Type type in
			Assembly.GetAssembly(typeof(T)).GetTypes()
			.Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T)))) {
			objects.Add((T)Activator.CreateInstance(type, constructorArgs));
		}
		return objects;
	}
}


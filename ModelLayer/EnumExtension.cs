using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ModelLayer
{
	public static class EnumExtension
	{
		//是否存在权限
		public static bool Has<T>(this System.Enum type, T value)
		{
			try
			{
			   return (((int)(object)type & (int)(object)value) == (int)(object)value);
		   }
		   catch
		   {
			   return false;
		   }
		}
		//判断权限
		public static bool Is<T>(this System.Enum type, T value)
		{
			try
			{
				return (int)(object)type == (int)(object)value;
			}
			catch
			{
				return false;
			}
		}
		//添加权限
		public static T Add<T>(this System.Enum type, T value)
		 {
			 try
			 {
				 return (T)(object)(((int)(object)type | (int)(object)value));
			 }
			 catch (Exception ex)
			 {
				 throw new ArgumentException(
					 string.Format(
						 "不能添加类型 '{0}'",
						 typeof(T).Name
						 ), ex);
			 }
		 }

		//移除权限
		public static T Remove<T>(this System.Enum type, T value)
		{
			try
			{
				return (T)(object)(((int)(object)type & ~(int)(object)value));
			}
			catch (Exception ex)
			{
				throw new ArgumentException(
					string.Format(
						"不能移除类型 '{0}'",
						typeof(T).Name
						), ex);
			}
		}
	}	
}
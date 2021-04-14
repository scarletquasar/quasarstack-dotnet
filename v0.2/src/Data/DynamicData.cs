using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace quasarStack.Data
{
    public static class DynamicData
    {
		/*
		 * ==================================================================
 		 *  INVENTORY
 		 * ==================================================================
 		 */

		public static string[] NewInv()
		{
			List<string> inventoryList = new List<string>();
			return inventoryList.ToArray();
		}

		public static string[] InvAdd(object whatToAdd, object data, int amount, string[] inventory)
		{
			List<string> inventoryList = new List<string>();
			string toAdd = EasyData.StringConvert(whatToAdd);
			string dataToAdd = EasyData.StringConvert(data);

			foreach (string item in inventory)
			{

				inventoryList.Add(item);
			}

			for (int timestoadd = amount; timestoadd > 0; timestoadd--)
			{
				inventoryList.Add("<<[" + toAdd + "]>>" + "<<{" + dataToAdd + "}>>");
			}

			return inventoryList.ToArray();
		}

		public static string ShowInv(string[] inventory)
		{
			string inventoryList = "";

			var q = from x in inventory
					group x by x into g
					let count = g.Count()
					orderby count descending
					select new { Value = g.Key, Count = count };
			foreach (var x in q)
			{
				inventoryList += Environment.NewLine + x.Value + "(" + x.Count + ")";
			}

			return inventoryList;
		}

		public static string[] MinusInv(string toMinus, string[] inventory)
		{
			List<string> inventoryList = new List<string>();
			foreach (string item in inventory)
			{
				if (item != toMinus) inventoryList.Add(item);
			}
			return inventoryList.ToArray();
		}

		public static string[] RemInv(string toMinus, string[] inventory)
		{
			List<string> inventoryList = new List<string>();
			foreach (string item in inventory)
			{
				if (!item.Contains(toMinus)) inventoryList.Add(item);
			}
			return inventoryList.ToArray();
		}

	}

}

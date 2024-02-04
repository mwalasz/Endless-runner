using System.Collections;
using System;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
	public static class SurrogateCoroutine
	{
		#region Static methods

		public static void StartCoroutine(IEnumerator routine)
		{
            Scheduler.StartCoroutine(routine);
		}

		public static void StopCoroutine(IEnumerator routine)
		{
            Scheduler.StopCoroutine(routine);
		}

		public static void WaitUntilAndInvoke(Func<bool> predicate, Action action)
		{
            Scheduler.StartCoroutine(WaitUntilAndInvokeInternal(predicate, action));
		}

		public static void WaitUntilAndInvoke(IEnumerator coroutine, Action action)
		{
            Scheduler.StartCoroutine(WaitUntilAndInvokeInternal(coroutine, action));
		}

		public static void WaitUntilAndInvoke(YieldInstruction instruction, Action action)
		{
			Scheduler.StartCoroutine(WaitUntilAndInvokeInternal(instruction, action));
		}

		public static void WaitForEndOfFrameAndInvoke(Action action)
		{
			Scheduler.StartCoroutine(WaitUntilAndInvokeInternal(new WaitForEndOfFrame(), action));
		}

		public static void Invoke(Action action, float delay)
		{
			Scheduler.StartCoroutine(WaitUntilAndInvokeInternal(new WaitForSeconds(delay), action));
		}
			
		#endregion

		#region Private static methods

		private static IEnumerator WaitUntilAndInvokeInternal(Func<bool> predicate, Action action)
		{
			yield return new WaitUntil(predicate);

			action();
		}

		private static IEnumerator WaitUntilAndInvokeInternal(IEnumerator coroutine, Action action)
		{
			yield return coroutine;

			action();
		}

		private static IEnumerator WaitUntilAndInvokeInternal(YieldInstruction instruction, Action action)
		{
			yield return instruction;

			action();
		}

		private static IEnumerator InvokeInternal(Action action, float delay)
		{
			yield return new WaitForSeconds(delay);

			action();
		}

		#endregion
    }
}
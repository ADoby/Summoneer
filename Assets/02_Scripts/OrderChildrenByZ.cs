using System.Collections.Generic;
using UnityEngine;

public class OrderChildrenByZ : MonoBehaviour
{
	#region Public Fields

	public bool ReverseOrder = true;
	public bool ReverseWorkingDirection = true;

	public SimpleLibrary.Timer UpdateTimer;

	#endregion Public Fields

	#region Private Fields

	private int currentIndex = -1;
	private int lastIndex = -1;
	private float pos;
	private SortedList<float, Transform> sortedChilds = new SortedList<float, Transform>();
	private int startedChildCount = -1;
	private Transform trans;

	#endregion Private Fields

	#region Public Methods

	public void Reset(int count = -1)
	{
		sortedChilds.Clear();
		currentIndex = 0;
		startedChildCount = count;
		lastIndex = -1;
	}

	#endregion Public Methods

	#region Private Methods

	private void Update()
	{
		if (UpdateTimer.UpdateAutoReset())
		{
			int count = transform.childCount;
			if (count == 0)
				return;
			if (startedChildCount != count)
			{
				//Child count changed
				Reset(count);
			}

			if (lastIndex == -1)
			{
				trans = transform.GetChild(currentIndex);
				pos = trans.position.z;
				if (!trans.gameObject.activeSelf)
					pos += 10000f;
				for (int i = 0; i < 100; i++)
				{
					if (!sortedChilds.ContainsKey(pos))
						break;
					pos += 0.01f;
				}

				sortedChilds.Add(pos, trans);
				currentIndex++;

				if (currentIndex >= startedChildCount)
				{
					if (ReverseWorkingDirection)
						lastIndex = 0;
					else
						lastIndex = sortedChilds.Count - 1;
				}
			}
			else
			{
				for (int i = 0; i < sortedChilds.Count; i++)
				{
					trans = sortedChilds[sortedChilds.Keys[lastIndex]];
					trans.SetSiblingIndex(ReverseOrder ? (sortedChilds.Count - lastIndex) : lastIndex);

					if (ReverseWorkingDirection)
						lastIndex++;
					else
						lastIndex--;

					if (lastIndex < 0 || lastIndex >= sortedChilds.Count)
					{
						break;
					}
				}

				Reset();
			}
		}
	}

	#endregion Private Methods
}
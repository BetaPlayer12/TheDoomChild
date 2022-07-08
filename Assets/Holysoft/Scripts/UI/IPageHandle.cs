using Holysoft.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Holysoft.Collections
{
    public interface IPageHandle
    {
        event EventAction<EventActionArgs> PageChange;

        int currentPage { get; }

        int GetTotalPages();
        void SetPage(int pageIndex);
        void NextPage();
        void PreviousPage();
    }

}
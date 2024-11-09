using Kitchen.Components;
using Kitchen;
using KitchenMods;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using TableNumbers.Components;
using TMPro;

namespace TableNumbers.Views
{
    public class TableNumberView : UpdatableObjectView<TableNumberView.ViewData>
    {
        public TextMeshPro Label;

        private void Start()
        {
            //Label = transform.Find("Label 1(Clone)").GetComponent<TextMeshPro>();
            if (Label != null)
                Label.transform.localPosition = new Vector3(0f, 0.45f, -0.55f);
        }

        private void Update()
        {
            if (Label != null)
                Label.transform.rotation = Quaternion.Euler(45, 0, 0);
        }
        protected override void UpdateData(TableNumberView.ViewData data)
        {
            if (Label != null)
            {
                Label.gameObject.SetActive(data.TableNumber != 0);
                Label.text = data.TableNumber.ToString();
            }
        }

        public class UpdateView : IncrementalViewSystemBase<ViewData>, IModSystem
        {
            private EntityQuery query;
            protected override void Initialise()
            {
                base.Initialise();
                query = GetEntityQuery(new QueryHelper().All(typeof(CLinkedView), typeof(CTableNumber)));
            }

            protected override void OnUpdate()
            {
                using var views = query.ToComponentDataArray<CLinkedView>(Allocator.Temp);
                using var tableNumbers = query.ToComponentDataArray<CTableNumber>(Allocator.Temp);

                for (var i = 0; i < views.Length; i++)
                {
                    var view = views[i];
                    var cTableNumber = tableNumbers[i];

                    SendUpdate(view, new ViewData
                    {
                        TableNumber = cTableNumber.TableNumber,
                    }, MessageType.SpecificViewUpdate);
                }
            }
        }

        [MessagePackObject(false)]
        public struct ViewData : ISpecificViewData, IViewData, IViewResponseData, IViewData.ICheckForChanges<ViewData>
        {
            [Key(0)] public int TableNumber;
            public IUpdatableObject GetRelevantSubview(IObjectView view) => view.GetSubView<TableNumberView>();

            public bool IsChangedFrom(ViewData check) => check.TableNumber != TableNumber;
        }
    }
}

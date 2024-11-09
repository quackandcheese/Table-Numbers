using Kitchen;
using KitchenMods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableNumbers.Components;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.UIElements;

namespace TableNumbers.Systems
{
    public class SetTableNumbers : GameSystemBase, IModSystem
    {
        private EntityQuery Tables;

        protected override void Initialise()
        {
            base.Initialise();
            Tables = GetEntityQuery(typeof(CTableNumber), typeof(CItemHolder));
        }

        protected override void OnUpdate()
        {
            if (Tables.IsEmpty)
            {
                return;
            }
            NativeArray<Entity> entities = Tables.ToEntityArray(Allocator.Temp);
            NativeArray<CPosition> positions = Tables.ToComponentDataArray<CPosition>(Allocator.Temp);

            for (int i = 0; i < positions.Length - 1; i++)
            {
                for (int j = i + 1; j < positions.Length; j++)
                {
                    // Compare positions to sort by z first (in descending order), 
                    // and if z is equal, by x (in ascending order)
                    if (positions[j].Position.z > positions[i].Position.z ||
                        (positions[j].Position.z == positions[i].Position.z && positions[j].Position.x < positions[i].Position.x))
                    {
                        // Swap positions
                        (positions[i], positions[j]) = (positions[j], positions[i]);

                        // Swap entities to keep them in sync
                        (entities[i], entities[j]) = (entities[j], entities[i]);
                    }
                }
            }


            for (int i = 0; i < entities.Length; i++)
            {
                Set(entities[i], new CTableNumber
                {
                    TableNumber = i + 1
                });
                
            }

            entities.Dispose();
            positions.Dispose();
        }
    }
}

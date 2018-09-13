using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace EntityExample {
    public class Spawner : ComponentSystem {
        private SettingsHandler _settings;
        private Random _rand = new Random(0x6E624EB7u);

        private Entity _root;

        protected override void OnCreateManager(int capacity) {
            _settings = Resources.Load<SettingsHandler>("DemoSettings");
            var entityCount = _settings.NodeCount * _settings.LeafCount + 1;
            Debug.Log(entityCount + 1);
            var entities = new NativeArray<Entity>(entityCount + 1, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
            var archetype = EntityManager.CreateArchetype(typeof(Position), typeof(Scale), typeof(MeshInstanceRenderer));
            EntityManager.CreateEntity(archetype, entities);
            _root = entities[0];
            EntityManager.SetComponentData(_root, new Scale {Value = 1});
            EntityManager.SetSharedComponentData(_root, _settings.NodeRenderer);
            for (var i = 0; i < _settings.NodeCount; i++) {
                var node = SetupEntity(entities[1 + i * _settings.LeafCount], _root, _settings.NodeRenderer, 0.25f);
                for (var j = 1; j < _settings.LeafCount; j++) {
                    SetupEntity(entities[1 + i * _settings.LeafCount + j], node, _settings.LeafRenderer, 0.25f);
                }
            }
        }


        public Entity SetupEntity(Entity entity, Entity parent, MeshInstanceRenderer renderer, float scale) {
            EntityManager.SetComponentData(entity, new Position {Value = _rand.NextFloat3Direction() * scale * 10});
            EntityManager.SetComponentData(entity, new Scale {Value = scale});
            EntityManager.SetSharedComponentData(entity, renderer);
            EntityManager.AddComponentData(EntityManager.CreateEntity(), new Attach {Parent = parent, Child = entity});
            return entity;
        }

        protected override void OnUpdate() {
            if (Input.GetKeyDown(KeyCode.Delete)) {
                EntityManager.DestroyEntity(_root);
            }
        }
    }
}
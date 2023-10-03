using System.Collections.Generic;
using nadena.dev.ndmf;
using UnityEngine;

namespace Anatawa12.AvatarOptimizer.Processors.TraceAndOptimizes
{
    class TraceAndOptimizeState
    {
        public bool Enabled;
        public bool FreezeBlendShape;
        public bool RemoveUnusedObjects;
        public bool MmdWorldCompatibility;

        public bool PreserveEndBone;
        public bool UseLegacyAnimatorParser;
        public HashSet<GameObject> Exclusions = new HashSet<GameObject>();
        public bool UseLegacyGC;
        public bool GCDebug;
        public bool NoConfigureMergeBone;

        public ImmutableModificationsContainer Modifications;

        public TraceAndOptimizeState()
        {
        }

        public void Initialize(TraceAndOptimize config)
        {
            FreezeBlendShape = config.freezeBlendShape;
            RemoveUnusedObjects = config.removeUnusedObjects;
            MmdWorldCompatibility = config.mmdWorldCompatibility;

            PreserveEndBone = config.preserveEndBone;

            UseLegacyAnimatorParser = !config.advancedAnimatorParser;
            Exclusions = new HashSet<GameObject>(config.advancedSettings.exclusions);
            UseLegacyGC = config.advancedSettings.useLegacyGc;
            GCDebug = config.advancedSettings.gcDebug;
            NoConfigureMergeBone = config.advancedSettings.noConfigureMergeBone;

            Enabled = true;
        }
    }

    internal class LoadTraceAndOptimizeConfiguration : Pass<LoadTraceAndOptimizeConfiguration>
    {
        public override string DisplayName => "T&O: Load Configuration";

        protected override void Execute(BuildContext context)
        {
            var config = context.AvatarRootObject.GetComponent<TraceAndOptimize>();
            if (config)
                context.GetState<TraceAndOptimizeState>().Initialize(config);
            Object.DestroyImmediate(config);
        }
    }

    internal class ParseAnimator : Pass<ParseAnimator>
    {
        public override string DisplayName => "T&O: Parse Animator";

        protected override void Execute(BuildContext context)
        {
            var state = context.GetState<TraceAndOptimizeState>();
            if (state.Enabled)
                state.Modifications = new AnimatorParser(state).GatherAnimationModifications(context);
        }
    }
}

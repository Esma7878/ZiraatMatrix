using System;

namespace ZiraatProje.Business
{
    public class ZiraatMatrixAiEngine
    {
        public SmartLeaveRecommendationService Leaves { get; } = new SmartLeaveRecommendationService();
        public SmartShiftAssistantService Shifts { get; } = new SmartShiftAssistantService();
        public SmartProjectAllocationService Projects { get; } = new SmartProjectAllocationService();
        public SmartExecutiveDigestService Digest { get; } = new SmartExecutiveDigestService();
        public SmartChatAssistantService Chat { get; } = new SmartChatAssistantService();

        private static readonly Lazy<ZiraatMatrixAiEngine> _instance = new Lazy<ZiraatMatrixAiEngine>(() => new ZiraatMatrixAiEngine());
        public static ZiraatMatrixAiEngine Instance => _instance.Value;
    }
}

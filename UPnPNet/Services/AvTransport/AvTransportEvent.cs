using System.Collections.Generic;

namespace UPnPNet.Services.AvTransport
{
	public class AvTransportEvent : UPnPLastChangeEvent
	{
		public int? InstanceId => TryGetIntValue("InstanceId");

		public AvTransportTransportState TransportState => AvTransportTransportState.GetByValue(TryGetValue("TransportState"));
		public AvTransportTransportStatus TransportStatus => AvTransportTransportStatus.GetByValue(TryGetValue("TransportStatus"));
		public string PlaybackStorageMedium => TryGetValue("PlaybackStorageMedium");
		public string RecordStorageMedium => TryGetValue("RecordStorageMedium");
		public string PossiblePlaybackStorageMedia => TryGetValue("PossiblePlaybackStorageMedia");
		public string PossibleRecordStorageMedia => TryGetValue("PossibleRecordStorageMedia");
		public AvTransportPlayMode CurrentPlayMode => AvTransportPlayMode.GetByValue(TryGetValue("CurrentPlayMode"));
		public int? TransportPlaySpeed => TryGetIntValue("TransportPlaySpeed");
		public string RecoardMediumWriteStatus => TryGetValue("RecoardMediumWriteStatus");
		public string CurrentRecordQualityMode => TryGetValue("CurrentRecordQualityMode");
		public string PossibleRecordQualityMode => TryGetValue("PossibleRecordQualityMode");
		public int? NumberOfTracks => TryGetIntValue("NumberOfTracks");
		public int? CurrentTrack => TryGetIntValue("CurrentTrack");
		public string CurrentTrackDuration => TryGetValue("CurrentTrackDuration");
		public string CurrentMediaDuration => TryGetValue("CurrentMediaDuration");
		public string CurrentTrackMetadata => TryGetValue("CurrentTrackMetadata");
		public string CurrentTrackUri => TryGetValue("CurrentTrackUri");
		public string AVTransportUri => TryGetValue("AVTransportUri");
		public string NextAVTransportUri => TryGetValue("NextAVTransportUri");
		public string CurrentTransportActions => TryGetValue("CurrentTransportActions");

		public AvTransportEvent(IList<UPnPLastChangeValue> values) : base(values)
		{
		}
	}
}
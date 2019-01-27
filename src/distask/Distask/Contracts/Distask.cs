// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: distask.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Distask.Contracts {

  /// <summary>Holder for reflection information generated from distask.proto</summary>
  public static partial class DistaskReflection {

    #region Descriptor
    /// <summary>File descriptor for distask.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static DistaskReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Cg1kaXN0YXNrLnByb3RvEhFkaXN0YXNrLmNvbnRyYWN0cyI2Cg5EaXN0YXNr",
            "UmVxdWVzdBIQCgh0YXNrTmFtZRgBIAEoCRISCgpwYXJhbWV0ZXJzGAIgAygJ",
            "IqMBCg9EaXN0YXNrUmVzcG9uc2USLQoGc3RhdHVzGAEgASgOMh0uZGlzdGFz",
            "ay5jb250cmFjdHMuU3RhdHVzQ29kZRIOCgZyZXN1bHQYAiABKAkSFAoMZXJy",
            "b3JNZXNzYWdlGAMgASgJEhIKCnN0YWNrVHJhY2UYBCABKAkSEQoJZXJyb3JU",
            "eXBlGAUgASgJEhQKDGVycm9yRGV0YWlscxgGIAEoCSJOChNSZWdpc3RyYXRp",
            "b25SZXF1ZXN0EgwKBG5hbWUYASABKAkSDQoFZ3JvdXAYAiABKAkSDAoEaG9z",
            "dBgDIAEoCRIMCgRwb3J0GAQgASgFIt0BChRSZWdpc3RyYXRpb25SZXNwb25z",
            "ZRItCgZzdGF0dXMYASABKA4yHS5kaXN0YXNrLmNvbnRyYWN0cy5TdGF0dXND",
            "b2RlEhUKDXJlamVjdE1lc3NhZ2UYAiABKAkSRAoGcmVhc29uGAMgASgOMjQu",
            "ZGlzdGFzay5jb250cmFjdHMuUmVnaXN0cmF0aW9uUmVzcG9uc2UuUmVqZWN0",
            "UmVhc29uIjkKDFJlamVjdFJlYXNvbhIICgROT05FEAASCwoHR0VORVJBTBAB",
            "EhIKDkFMUkVBRFlfRVhJU1RTEAIqMQoKU3RhdHVzQ29kZRILCgdTVUNDRVNT",
            "EAASCwoHV0FSTklORxABEgkKBUVSUk9SEAIyZAoORGlzdGFza1NlcnZpY2US",
            "UgoHRXhlY3V0ZRIhLmRpc3Rhc2suY29udHJhY3RzLkRpc3Rhc2tSZXF1ZXN0",
            "GiIuZGlzdGFzay5jb250cmFjdHMuRGlzdGFza1Jlc3BvbnNlIgAyewoaRGlz",
            "dGFza1JlZ2lzdHJhdGlvblNlcnZpY2USXQoIUmVnaXN0ZXISJi5kaXN0YXNr",
            "LmNvbnRyYWN0cy5SZWdpc3RyYXRpb25SZXF1ZXN0GicuZGlzdGFzay5jb250",
            "cmFjdHMuUmVnaXN0cmF0aW9uUmVzcG9uc2UiAGIGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(new[] {typeof(global::Distask.Contracts.StatusCode), }, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Distask.Contracts.DistaskRequest), global::Distask.Contracts.DistaskRequest.Parser, new[]{ "TaskName", "Parameters" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Distask.Contracts.DistaskResponse), global::Distask.Contracts.DistaskResponse.Parser, new[]{ "Status", "Result", "ErrorMessage", "StackTrace", "ErrorType", "ErrorDetails" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Distask.Contracts.RegistrationRequest), global::Distask.Contracts.RegistrationRequest.Parser, new[]{ "Name", "Group", "Host", "Port" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Distask.Contracts.RegistrationResponse), global::Distask.Contracts.RegistrationResponse.Parser, new[]{ "Status", "RejectMessage", "Reason" }, null, new[]{ typeof(global::Distask.Contracts.RegistrationResponse.Types.RejectReason) }, null)
          }));
    }
    #endregion

  }
  #region Enums
  public enum StatusCode {
    [pbr::OriginalName("SUCCESS")] Success = 0,
    [pbr::OriginalName("WARNING")] Warning = 1,
    [pbr::OriginalName("ERROR")] Error = 2,
  }

  #endregion

  #region Messages
  public sealed partial class DistaskRequest : pb::IMessage<DistaskRequest> {
    private static readonly pb::MessageParser<DistaskRequest> _parser = new pb::MessageParser<DistaskRequest>(() => new DistaskRequest());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<DistaskRequest> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Distask.Contracts.DistaskReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DistaskRequest() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DistaskRequest(DistaskRequest other) : this() {
      taskName_ = other.taskName_;
      parameters_ = other.parameters_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DistaskRequest Clone() {
      return new DistaskRequest(this);
    }

    /// <summary>Field number for the "taskName" field.</summary>
    public const int TaskNameFieldNumber = 1;
    private string taskName_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string TaskName {
      get { return taskName_; }
      set {
        taskName_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "parameters" field.</summary>
    public const int ParametersFieldNumber = 2;
    private static readonly pb::FieldCodec<string> _repeated_parameters_codec
        = pb::FieldCodec.ForString(18);
    private readonly pbc::RepeatedField<string> parameters_ = new pbc::RepeatedField<string>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<string> Parameters {
      get { return parameters_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as DistaskRequest);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(DistaskRequest other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (TaskName != other.TaskName) return false;
      if(!parameters_.Equals(other.parameters_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (TaskName.Length != 0) hash ^= TaskName.GetHashCode();
      hash ^= parameters_.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (TaskName.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(TaskName);
      }
      parameters_.WriteTo(output, _repeated_parameters_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (TaskName.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(TaskName);
      }
      size += parameters_.CalculateSize(_repeated_parameters_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(DistaskRequest other) {
      if (other == null) {
        return;
      }
      if (other.TaskName.Length != 0) {
        TaskName = other.TaskName;
      }
      parameters_.Add(other.parameters_);
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            TaskName = input.ReadString();
            break;
          }
          case 18: {
            parameters_.AddEntriesFrom(input, _repeated_parameters_codec);
            break;
          }
        }
      }
    }

  }

  public sealed partial class DistaskResponse : pb::IMessage<DistaskResponse> {
    private static readonly pb::MessageParser<DistaskResponse> _parser = new pb::MessageParser<DistaskResponse>(() => new DistaskResponse());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<DistaskResponse> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Distask.Contracts.DistaskReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DistaskResponse() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DistaskResponse(DistaskResponse other) : this() {
      status_ = other.status_;
      result_ = other.result_;
      errorMessage_ = other.errorMessage_;
      stackTrace_ = other.stackTrace_;
      errorType_ = other.errorType_;
      errorDetails_ = other.errorDetails_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DistaskResponse Clone() {
      return new DistaskResponse(this);
    }

    /// <summary>Field number for the "status" field.</summary>
    public const int StatusFieldNumber = 1;
    private global::Distask.Contracts.StatusCode status_ = 0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Distask.Contracts.StatusCode Status {
      get { return status_; }
      set {
        status_ = value;
      }
    }

    /// <summary>Field number for the "result" field.</summary>
    public const int ResultFieldNumber = 2;
    private string result_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Result {
      get { return result_; }
      set {
        result_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "errorMessage" field.</summary>
    public const int ErrorMessageFieldNumber = 3;
    private string errorMessage_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string ErrorMessage {
      get { return errorMessage_; }
      set {
        errorMessage_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "stackTrace" field.</summary>
    public const int StackTraceFieldNumber = 4;
    private string stackTrace_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string StackTrace {
      get { return stackTrace_; }
      set {
        stackTrace_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "errorType" field.</summary>
    public const int ErrorTypeFieldNumber = 5;
    private string errorType_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string ErrorType {
      get { return errorType_; }
      set {
        errorType_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "errorDetails" field.</summary>
    public const int ErrorDetailsFieldNumber = 6;
    private string errorDetails_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string ErrorDetails {
      get { return errorDetails_; }
      set {
        errorDetails_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as DistaskResponse);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(DistaskResponse other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Status != other.Status) return false;
      if (Result != other.Result) return false;
      if (ErrorMessage != other.ErrorMessage) return false;
      if (StackTrace != other.StackTrace) return false;
      if (ErrorType != other.ErrorType) return false;
      if (ErrorDetails != other.ErrorDetails) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Status != 0) hash ^= Status.GetHashCode();
      if (Result.Length != 0) hash ^= Result.GetHashCode();
      if (ErrorMessage.Length != 0) hash ^= ErrorMessage.GetHashCode();
      if (StackTrace.Length != 0) hash ^= StackTrace.GetHashCode();
      if (ErrorType.Length != 0) hash ^= ErrorType.GetHashCode();
      if (ErrorDetails.Length != 0) hash ^= ErrorDetails.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Status != 0) {
        output.WriteRawTag(8);
        output.WriteEnum((int) Status);
      }
      if (Result.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(Result);
      }
      if (ErrorMessage.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(ErrorMessage);
      }
      if (StackTrace.Length != 0) {
        output.WriteRawTag(34);
        output.WriteString(StackTrace);
      }
      if (ErrorType.Length != 0) {
        output.WriteRawTag(42);
        output.WriteString(ErrorType);
      }
      if (ErrorDetails.Length != 0) {
        output.WriteRawTag(50);
        output.WriteString(ErrorDetails);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Status != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) Status);
      }
      if (Result.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Result);
      }
      if (ErrorMessage.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(ErrorMessage);
      }
      if (StackTrace.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(StackTrace);
      }
      if (ErrorType.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(ErrorType);
      }
      if (ErrorDetails.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(ErrorDetails);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(DistaskResponse other) {
      if (other == null) {
        return;
      }
      if (other.Status != 0) {
        Status = other.Status;
      }
      if (other.Result.Length != 0) {
        Result = other.Result;
      }
      if (other.ErrorMessage.Length != 0) {
        ErrorMessage = other.ErrorMessage;
      }
      if (other.StackTrace.Length != 0) {
        StackTrace = other.StackTrace;
      }
      if (other.ErrorType.Length != 0) {
        ErrorType = other.ErrorType;
      }
      if (other.ErrorDetails.Length != 0) {
        ErrorDetails = other.ErrorDetails;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            status_ = (global::Distask.Contracts.StatusCode) input.ReadEnum();
            break;
          }
          case 18: {
            Result = input.ReadString();
            break;
          }
          case 26: {
            ErrorMessage = input.ReadString();
            break;
          }
          case 34: {
            StackTrace = input.ReadString();
            break;
          }
          case 42: {
            ErrorType = input.ReadString();
            break;
          }
          case 50: {
            ErrorDetails = input.ReadString();
            break;
          }
        }
      }
    }

  }

  public sealed partial class RegistrationRequest : pb::IMessage<RegistrationRequest> {
    private static readonly pb::MessageParser<RegistrationRequest> _parser = new pb::MessageParser<RegistrationRequest>(() => new RegistrationRequest());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<RegistrationRequest> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Distask.Contracts.DistaskReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public RegistrationRequest() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public RegistrationRequest(RegistrationRequest other) : this() {
      name_ = other.name_;
      group_ = other.group_;
      host_ = other.host_;
      port_ = other.port_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public RegistrationRequest Clone() {
      return new RegistrationRequest(this);
    }

    /// <summary>Field number for the "name" field.</summary>
    public const int NameFieldNumber = 1;
    private string name_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Name {
      get { return name_; }
      set {
        name_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "group" field.</summary>
    public const int GroupFieldNumber = 2;
    private string group_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Group {
      get { return group_; }
      set {
        group_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "host" field.</summary>
    public const int HostFieldNumber = 3;
    private string host_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Host {
      get { return host_; }
      set {
        host_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "port" field.</summary>
    public const int PortFieldNumber = 4;
    private int port_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Port {
      get { return port_; }
      set {
        port_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as RegistrationRequest);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(RegistrationRequest other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Name != other.Name) return false;
      if (Group != other.Group) return false;
      if (Host != other.Host) return false;
      if (Port != other.Port) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Name.Length != 0) hash ^= Name.GetHashCode();
      if (Group.Length != 0) hash ^= Group.GetHashCode();
      if (Host.Length != 0) hash ^= Host.GetHashCode();
      if (Port != 0) hash ^= Port.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Name.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Name);
      }
      if (Group.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(Group);
      }
      if (Host.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(Host);
      }
      if (Port != 0) {
        output.WriteRawTag(32);
        output.WriteInt32(Port);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Name.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Name);
      }
      if (Group.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Group);
      }
      if (Host.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Host);
      }
      if (Port != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Port);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(RegistrationRequest other) {
      if (other == null) {
        return;
      }
      if (other.Name.Length != 0) {
        Name = other.Name;
      }
      if (other.Group.Length != 0) {
        Group = other.Group;
      }
      if (other.Host.Length != 0) {
        Host = other.Host;
      }
      if (other.Port != 0) {
        Port = other.Port;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            Name = input.ReadString();
            break;
          }
          case 18: {
            Group = input.ReadString();
            break;
          }
          case 26: {
            Host = input.ReadString();
            break;
          }
          case 32: {
            Port = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  public sealed partial class RegistrationResponse : pb::IMessage<RegistrationResponse> {
    private static readonly pb::MessageParser<RegistrationResponse> _parser = new pb::MessageParser<RegistrationResponse>(() => new RegistrationResponse());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<RegistrationResponse> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Distask.Contracts.DistaskReflection.Descriptor.MessageTypes[3]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public RegistrationResponse() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public RegistrationResponse(RegistrationResponse other) : this() {
      status_ = other.status_;
      rejectMessage_ = other.rejectMessage_;
      reason_ = other.reason_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public RegistrationResponse Clone() {
      return new RegistrationResponse(this);
    }

    /// <summary>Field number for the "status" field.</summary>
    public const int StatusFieldNumber = 1;
    private global::Distask.Contracts.StatusCode status_ = 0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Distask.Contracts.StatusCode Status {
      get { return status_; }
      set {
        status_ = value;
      }
    }

    /// <summary>Field number for the "rejectMessage" field.</summary>
    public const int RejectMessageFieldNumber = 2;
    private string rejectMessage_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string RejectMessage {
      get { return rejectMessage_; }
      set {
        rejectMessage_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "reason" field.</summary>
    public const int ReasonFieldNumber = 3;
    private global::Distask.Contracts.RegistrationResponse.Types.RejectReason reason_ = 0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Distask.Contracts.RegistrationResponse.Types.RejectReason Reason {
      get { return reason_; }
      set {
        reason_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as RegistrationResponse);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(RegistrationResponse other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Status != other.Status) return false;
      if (RejectMessage != other.RejectMessage) return false;
      if (Reason != other.Reason) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Status != 0) hash ^= Status.GetHashCode();
      if (RejectMessage.Length != 0) hash ^= RejectMessage.GetHashCode();
      if (Reason != 0) hash ^= Reason.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Status != 0) {
        output.WriteRawTag(8);
        output.WriteEnum((int) Status);
      }
      if (RejectMessage.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(RejectMessage);
      }
      if (Reason != 0) {
        output.WriteRawTag(24);
        output.WriteEnum((int) Reason);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Status != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) Status);
      }
      if (RejectMessage.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(RejectMessage);
      }
      if (Reason != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) Reason);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(RegistrationResponse other) {
      if (other == null) {
        return;
      }
      if (other.Status != 0) {
        Status = other.Status;
      }
      if (other.RejectMessage.Length != 0) {
        RejectMessage = other.RejectMessage;
      }
      if (other.Reason != 0) {
        Reason = other.Reason;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            status_ = (global::Distask.Contracts.StatusCode) input.ReadEnum();
            break;
          }
          case 18: {
            RejectMessage = input.ReadString();
            break;
          }
          case 24: {
            reason_ = (global::Distask.Contracts.RegistrationResponse.Types.RejectReason) input.ReadEnum();
            break;
          }
        }
      }
    }

    #region Nested types
    /// <summary>Container for nested types declared in the RegistrationResponse message type.</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static partial class Types {
      public enum RejectReason {
        [pbr::OriginalName("NONE")] None = 0,
        [pbr::OriginalName("GENERAL")] General = 1,
        [pbr::OriginalName("ALREADY_EXISTS")] AlreadyExists = 2,
      }

    }
    #endregion

  }

  #endregion

}

#endregion Designer generated code

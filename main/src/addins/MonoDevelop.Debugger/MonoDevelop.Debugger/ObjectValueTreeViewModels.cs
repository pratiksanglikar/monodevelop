//
// ObjectValueTreeViewModels.cs
//
// Author:
//       gregm <gregm@microsoft.com>
//
// Copyright (c) 2019 Microsoft
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Mono.Debugging.Client;
using MonoDevelop.Components;
using MonoDevelop.Core;

namespace MonoDevelop.Debugger
{
	/// <summary>
	/// Represents an object value returned from the debugger 
	/// </summary>
	public interface IObjectValueNode
	{
		/// <summary>
		/// Gets the path of the node from the root.
		/// </summary>
		string Path { get; }

		/// <summary>
		/// Gets the path of the parent of node from the root.
		/// </summary>
		string ParentPath { get; }

		/// <summary>
		/// Gets the collection of children that have been loaded from the debugger
		/// </summary>
		IReadOnlyList<IObjectValueNode> Children { get; }

		/// <summary>
		/// Gets the name of the object
		/// </summary>
		string Name { get; }

		// TODO: make the setter private and get the node to do the expansion
		/// <summary>
		/// Gets or sets a value indicating whether the node is expanded
		/// </summary>
		bool IsExpanded { get; set; }

		/// <summary>
		/// Gets a value indicating whether the node has children or not.
		/// The children may not yet be loaded and Children may return 0 items if not loaded
		/// </summary>
		bool HasChildren { get; }

		/// <summary>
		/// Gets a value indicating whether all children for this node have been loaded from the debugger
		/// </summary>
		bool ChildrenLoaded { get; }

		/// <summary>
		/// Gets a value indicating whether the object is an enumerable
		/// </summary>
		bool IsEnumerable { get; }

		/// <summary>
		/// Gets a value indicating whether the debugger is still evaluating the object
		/// </summary>
		bool IsEvaluating { get; }

		/// <summary>
		/// Gets a value indicating whether the value can be edited by the user or not
		/// </summary>
		bool CanEdit { get; }


		bool IsUnknown { get; }
		bool IsReadOnly { get; }
		bool IsError { get; }
		bool IsNotSupported { get; }
		string Value { get; }
		bool IsImplicitNotSupported { get; }
		ObjectValueFlags Flags { get; }
		bool IsNull { get; }
		bool IsPrimitive { get; }
		string TypeName { get; }
		string DisplayValue { get; }
		bool CanRefresh { get; }
		bool HasFlag (ObjectValueFlags flag);

		/// <summary>
		/// Fired when the value of the object has changed
		/// </summary>
		event EventHandler ValueChanged;

		/// <summary>
		/// Attempts to set the value of the node to newValue
		/// </summary>
		void SetValue (string newValue);

		/// <summary>
		/// Tells the object to refresh its values from the debugger
		/// </summary>
		void Refresh ();

		/// <summary>
		/// Asynchronously loads all children for the node into Children.
		/// The task will complete immediately if all the children have previously been loaded and
		/// the debugger will not be re-queried
		/// </summary>
		Task<int> LoadChildrenAsync (CancellationToken cancellationToken);

		/// <summary>
		/// Asynchronously loads a range of at most count children for the node into Children.
		/// Subsequent calls will load an additional count children into Children until all children
		/// have been loaded.
		/// The task will complete immediately if all the children have previously been loaded and
		/// the debugger will not be re-queried. 
		/// </summary>
		Task<int> LoadChildrenAsync (int count, CancellationToken cancellationToken);
	}

	interface IEvaluatingGroupObjectValueNode
	{
		/// <summary>
		/// Gets a value indicating whether this object that was evaulating was evaluating a group
		/// of objects, such as locals, and whether we should replace the node with a set of new
		/// nodes once evaulation has completed
		/// </summary>
		bool IsEvaluatingGroup { get; }

		/// <summary>
		/// Get an array of new objectvalue nodes that should replace the current node in the tree
		/// </summary>
		IObjectValueNode [] GetEvaluationGroupReplacementNodes ();
	}

	/// <summary>
	/// Base class for IObjectValueNode implementations
	/// </summary>
	public abstract class AbstractObjectValueNode : IObjectValueNode
	{
		readonly List<IObjectValueNode> children = new List<IObjectValueNode> ();
		bool allChildrenLoaded;

		protected AbstractObjectValueNode (string parentPath, string name)
		{
			this.Name = name;
			this.ParentPath = parentPath;
			if (parentPath.EndsWith ("/", StringComparison.OrdinalIgnoreCase)) {
				Path = parentPath + name;
			} else {
				Path = parentPath + "/" + name;
			}
		}

		public string Path { get; }
		public string ParentPath { get; }
		public string Name { get; }
		public IReadOnlyList<IObjectValueNode> Children => children;
		public virtual bool IsExpanded { get; set; }
		public virtual bool HasChildren => false;
		public bool ChildrenLoaded => allChildrenLoaded;
		public virtual bool IsEnumerable => false;
		public virtual bool IsEvaluating => false;
		public virtual bool CanEdit => false;



		public virtual string DisplayValue => string.Empty;


		public virtual bool IsUnknown => false;
		public virtual bool IsReadOnly => false;
		public virtual bool IsError => false;
		public virtual bool IsNotSupported => false;
		public virtual string Value => string.Empty;
		public virtual bool IsImplicitNotSupported => false;
		public virtual ObjectValueFlags Flags => ObjectValueFlags.None;
		public virtual bool IsNull => false;
		public virtual bool IsPrimitive => false;
		public virtual string TypeName => string.Empty;
		public virtual bool CanRefresh => false;
		public virtual bool HasFlag (ObjectValueFlags flag) => false;


		public event EventHandler ValueChanged;

		public virtual void SetValue (string newValue)
		{
		}

		public virtual void Refresh ()
		{
		}

		public async Task<int> LoadChildrenAsync (CancellationToken cancellationToken)
		{
			if (!allChildrenLoaded) {
				var loadedChildren = await OnLoadChildrenAsync (cancellationToken);
				AddValues (loadedChildren);

				allChildrenLoaded = true;
				return loadedChildren.Count ();
			}

			return 0;
		}

		public async Task<int> LoadChildrenAsync (int count, CancellationToken cancellationToken)
		{
			if (!allChildrenLoaded) {
				var loadedChildren = await OnLoadChildrenAsync (children.Count, count, cancellationToken);
				AddValues (loadedChildren.Item1);

				allChildrenLoaded = loadedChildren.Item2;
				return loadedChildren.Item1.Count ();
			}

			return 0;
		}

		protected void AddValues (IEnumerable<IObjectValueNode> values)
		{
			this.children.AddRange (values);
		}

		protected void ClearChildren ()
		{
			children.Clear ();
			allChildrenLoaded = false;
		}

		protected virtual Task<IEnumerable<IObjectValueNode>> OnLoadChildrenAsync (CancellationToken cancellationToken)
		{
			return Task.FromResult (Enumerable.Empty<IObjectValueNode> ());
		}

		/// <summary>
		/// Returns the children that were loaded and a bool indicating whether all children have now been loaded
		/// </summary>
		protected virtual Task<Tuple<IEnumerable<IObjectValueNode>, bool>> OnLoadChildrenAsync (int index, int count, CancellationToken cancellationToken)
		{
			return Task.FromResult (Tuple.Create(Enumerable.Empty<IObjectValueNode> (), true));
		}

		protected void OnValueChanged (EventArgs e)
		{
			ValueChanged?.Invoke (this, e);
		}
	}

	/// <summary>
	/// Represents a node in a tree structure that holds ObjectValue from the debugger.
	/// </summary>
	public sealed class ObjectValueNode : AbstractObjectValueNode, IEvaluatingGroupObjectValueNode
	{
		readonly string parentPath;

		public ObjectValueNode (ObjectValue value, string parentPath) : base (parentPath, value.Name)
		{
			this.parentPath = parentPath;
			DebuggerObject = value;

			value.ValueChanged += OnDebuggerValueChanged;
		}

		// TODO: try and make this private
		public ObjectValue DebuggerObject { get; }


		public override bool HasChildren => DebuggerObject.HasChildren;
		public override bool IsEnumerable => DebuggerObject.Flags.HasFlag (ObjectValueFlags.IEnumerable);
		public override bool IsEvaluating => DebuggerObject.IsEvaluating;
		public bool IsEvaluatingGroup => DebuggerObject.IsEvaluatingGroup;
		public override bool CanEdit => GetCanEdit();


		public override bool IsUnknown => DebuggerObject.IsUnknown;
		public override bool IsReadOnly => DebuggerObject.IsReadOnly;
		public override bool IsError => DebuggerObject.IsError;
		public override bool IsNotSupported => DebuggerObject.IsNotSupported;
		public override string Value => DebuggerObject.Value;
		public override bool IsImplicitNotSupported => DebuggerObject.IsImplicitNotSupported;
		public override ObjectValueFlags Flags => DebuggerObject.Flags;
		public override bool IsNull => DebuggerObject.IsNull;
		public override bool IsPrimitive => DebuggerObject.IsPrimitive;
		public override string TypeName => DebuggerObject.TypeName;
		public override string DisplayValue => DebuggerObject.DisplayValue;
		public override bool CanRefresh => true;//DebuggerObject.CanRefresh;
		public override bool HasFlag (ObjectValueFlags flag) => DebuggerObject.HasFlag (flag);

		public IObjectValueNode [] GetEvaluationGroupReplacementNodes ()
		{
			var result = new IObjectValueNode [DebuggerObject.ArrayCount];
			for (int i = 0; i < result.Length; i++) {
				result [i] = new ObjectValueNode (DebuggerObject.GetArrayItem (i), parentPath);
			}

			return result;
		}

		public override void SetValue (string newValue)
		{
			this.DebuggerObject.Value = newValue;
		}

		public override void Refresh ()
		{
			this.DebuggerObject.Refresh ();
		}

		protected override async Task<IEnumerable<IObjectValueNode>> OnLoadChildrenAsync (CancellationToken cancellationToken)
		{
			var childValues = await GetChildrenAsync (DebuggerObject, cancellationToken);

			return childValues.Select (x => new ObjectValueNode (x, Path));
		}

		protected override async Task<Tuple<IEnumerable<IObjectValueNode>, bool>> OnLoadChildrenAsync (int index, int count, CancellationToken cancellationToken)
		{
			var childValues = await GetChildrenAsync (DebuggerObject, index, count, cancellationToken);
			var result = childValues.Select (x => new ObjectValueNode (x, Path));

			// if we returned less that we asked for, we assume we've now loaded all children
			return Tuple.Create<IEnumerable<IObjectValueNode>, bool> (result, childValues.Length < count);
		}


		static Task<ObjectValue []> GetChildrenAsync (ObjectValue value, CancellationToken cancellationToken)
		{
			return Task.Factory.StartNew (delegate (object arg) {
				try {
					return ((ObjectValue) arg).GetAllChildren ();
				} catch (Exception ex) {
					// Note: this should only happen if someone breaks ObjectValue.GetAllChildren()
					LoggingService.LogError ("Failed to get ObjectValue children.", ex);
					return new ObjectValue [0];
				}
			}, value, cancellationToken, TaskCreationOptions.None, TaskScheduler.Default);
		}

		static Task<ObjectValue []> GetChildrenAsync (ObjectValue value, int index, int count, CancellationToken cancellationToken)
		{
			return Task.Factory.StartNew (delegate (object arg) {
				try {
					return ((ObjectValue)arg).GetRangeOfChildren (index, count);
				} catch (Exception ex) {
					// Note: this should only happen if someone breaks ObjectValue.GetAllChildren()
					LoggingService.LogError ("Failed to get ObjectValue range of children.", ex);
					return new ObjectValue [0];
				}
			}, value, cancellationToken, TaskCreationOptions.None, TaskScheduler.Default);
		}

		void OnDebuggerValueChanged (object sender, EventArgs e)
		{
			OnValueChanged (e);
		}

		bool GetCanEdit()
		{
			bool canEdit;
			var val = this.DebuggerObject;

			if (val.IsUnknown) {
				//if (frame != null) {
				//	canEdit = false;
				//} else {
					canEdit = !val.IsReadOnly;
				//}
			} else if (val.IsError || val.IsNotSupported) {
				canEdit = false;
			} else if (val.IsImplicitNotSupported) {
				canEdit = false;
			} else if (val.IsEvaluating) {
				canEdit = false;
			} else if (val.Flags.HasFlag (ObjectValueFlags.IEnumerable)) {
				canEdit = false;
			} else {
				canEdit = val.IsPrimitive && !val.IsReadOnly;
			}

			return canEdit;
		}
	}

	/// <summary>
	/// Special node used as the root of the treeview. 
	/// </summary>
	sealed class RootObjectValueNode : AbstractObjectValueNode
	{
		public RootObjectValueNode () : base (string.Empty, string.Empty)
		{
			this.IsExpanded = true;
		}

		public override bool HasChildren => true;

		public new void AddValues (IEnumerable<IObjectValueNode> values)
		{
			base.AddValues (values);
		}
	}

	/// <summary>
	/// Special node used to indicate that more values are available. 
	/// </summary>
	sealed class ShowMoreValuesObjectValueNode : AbstractObjectValueNode
	{
		public ShowMoreValuesObjectValueNode (IObjectValueNode enumerableNode) : base (enumerableNode.Path, string.Empty)
		{
			EnumerableNode = enumerableNode;
		}

		public override bool IsEnumerable => true;

		public IObjectValueNode EnumerableNode { get; }
	}

	#region Mocking support abstractions
	public interface IDebuggerService
	{
		bool IsConnected { get; }
		bool IsPaused { get; }
		void NotifyVariableChanged ();
	}


	public interface IStackFrame
	{

	}

	sealed class ProxyDebuggerService : IDebuggerService
	{
		public bool IsConnected => DebuggingService.IsConnected;

		public bool IsPaused => DebuggingService.IsPaused;

		public void NotifyVariableChanged()
		{
			DebuggingService.NotifyVariableChanged ();
		}
	}

	sealed class ProxyStackFrame : IStackFrame
	{
		public ProxyStackFrame (StackFrame frame)
		{
			StackFrame = frame;
		}

		public StackFrame StackFrame {
			get; private set;
		}
	}
	#endregion

	#region Event classes
	public abstract class NodeEventArgs : EventArgs
	{
		protected NodeEventArgs (IObjectValueNode node)
		{
			Node = node;
		}

		public IObjectValueNode Node {
			get; private set;
		}
	}

	public sealed class NodeEvaluationCompletedEventArgs : NodeEventArgs
	{
		public NodeEvaluationCompletedEventArgs (IObjectValueNode node, IObjectValueNode[] replacementNodes) : base (node)
		{
			this.ReplacementNodes = replacementNodes;
		}

		/// <summary>
		/// Gets an array of nodes that should be used to replace the node that finished evaluating.
		/// Some sets of values, like local variables, frame locals and the like are fetched asynchronously
		/// and may take some time to fetch. In this case, a single object is returned that is a place holder
		/// for 0 or more values that should be expanded in the place of the evaluating node.
		/// </summary>
		public IObjectValueNode [] ReplacementNodes { get; }
	}

	/// <summary>
	/// Event args for when a node has been expanded
	/// </summary>
	public sealed class NodeExpandedEventArgs : NodeEventArgs
	{
		public NodeExpandedEventArgs (IObjectValueNode node) : base (node)
		{
		}
	}

	public sealed class ChildrenChangedEventArgs : NodeEventArgs
	{
		public ChildrenChangedEventArgs (IObjectValueNode node, int index, int count) : base (node)
		{
			Index = index;
			Count = count;
		}

		/// <summary>
		/// Gets the count of child nodes that were loaded
		/// </summary>
		public int Count { get; }

		/// <summary>
		/// Gets the index of the first child that was loaded
		/// </summary>
		public int Index { get; }
	}

	public sealed class NodeChangedEventArgs : NodeEventArgs
	{
		public NodeChangedEventArgs (IObjectValueNode node) : base (node)
		{
		}
	}
	#endregion
}
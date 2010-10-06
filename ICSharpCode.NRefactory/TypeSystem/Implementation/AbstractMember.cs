﻿// Copyright (c) 2010 AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under MIT X11 license (for details please see \doc\license.txt)

using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Base class for <see cref="IMember"/> implementations.
	/// </summary>
	public abstract class AbstractMember : AbstractFreezable, IMember
	{
		ITypeDefinition declaringTypeDefinition;
		ITypeReference returnType = SharedTypes.UnknownType;
		IList<IAttribute> attributes;
		IList<IExplicitInterfaceImplementation> interfaceImplementations;
		DomRegion region;
		DomRegion bodyRegion;
		string name;
		
		// 1 byte per enum + 2 bytes for flags
		Accessibility accessibility;
		EntityType entityType;
		protected BitVector16 flags;
		const ushort FlagSealed    = 0x0001;
		const ushort FlagAbstract  = 0x0002;
		const ushort FlagShadowing = 0x0004;
		const ushort FlagSynthetic = 0x0008;
		const ushort FlagVirtual   = 0x0010;
		const ushort FlagOverride  = 0x0020;
		const ushort FlagStatic    = 0x0040;
		// Flags of form 0xY000 are reserved for use in derived classes (DefaultMethod etc.)
		
		protected override void FreezeInternal()
		{
			attributes = FreezeList(attributes);
			interfaceImplementations = FreezeList(interfaceImplementations);
			base.FreezeInternal();
		}
		
		protected AbstractMember(ITypeDefinition declaringTypeDefinition, string name, EntityType entityType)
		{
			if (declaringTypeDefinition == null)
				throw new ArgumentNullException("declaringTypeDefinition");
			if (name == null)
				throw new ArgumentNullException("name");
			this.declaringTypeDefinition = declaringTypeDefinition;
			this.entityType = entityType;
			this.name = name;
		}
		
		/* do we really need copy constructor (for specialized members?)
		protected AbstractMember(IMember member)
		{
			if (member == null)
				throw new ArgumentNullException("member");
			this.declaringTypeDefinition = member.DeclaringTypeDefinition;
			this.returnType = member.ReturnType;
			this.attributes = CopyList(member.Attributes);
			this.interfaceImplementations = CopyList(member.InterfaceImplementations);
			this.region = member.Region;
			this.bodyRegion = member.BodyRegion;
			this.name = member.Name;
			this.accessibility = member.Accessibility;
			this.IsSealed = member.IsSealed;
			this.IsAbstract = member.IsAbstract;
			this.IsShadowing = member.IsShadowing;
			this.IsSynthetic = member.IsSynthetic;
			this.IsVirtual = member.IsVirtual;
			this.IsOverride = member.IsOverride;
			this.IsStatic = member.IsStatic;
		}
		
		static IList<T> CopyList<T>(IList<T> inputList)
		{
			if (inputList.Count == 0)
				return null;
			else
				return new List<T>(inputList);
		}
		 */
		
		public ITypeDefinition DeclaringTypeDefinition {
			get { return declaringTypeDefinition; }
		}
		
		public virtual IType DeclaringType {
			get { return declaringTypeDefinition; }
		}
		
		public virtual IMember GenericMember {
			get { return null; }
		}
		
		public ITypeReference ReturnType {
			get { return returnType; }
			set {
				CheckBeforeMutation();
				if (value == null)
					throw new ArgumentNullException();
				returnType = value;
			}
		}
		
		public IList<IExplicitInterfaceImplementation> InterfaceImplementations {
			get {
				if (interfaceImplementations == null)
					interfaceImplementations = new List<IExplicitInterfaceImplementation>();
				return interfaceImplementations;
			}
		}
		
		public bool IsVirtual {
			get { return flags[FlagVirtual]; }
			set {
				CheckBeforeMutation();
				flags[FlagVirtual] = value;
			}
		}
		
		public bool IsOverride {
			get { return flags[FlagOverride]; }
			set {
				CheckBeforeMutation();
				flags[FlagOverride] = value;
			}
		}
		
		public bool IsOverridable {
			get {
				return (IsVirtual || IsOverride) && !IsSealed;
			}
		}
		
		public EntityType EntityType {
			get { return entityType; }
			set {
				CheckBeforeMutation();
				entityType = value;
			}
		}
		
		public DomRegion Region {
			get { return region; }
			set {
				CheckBeforeMutation();
				region = value;
			}
		}
		
		public DomRegion BodyRegion {
			get { return bodyRegion; }
			set {
				CheckBeforeMutation();
				bodyRegion = value;
			}
		}
		
		public IList<IAttribute> Attributes {
			get {
				if (attributes == null)
					attributes = new List<IAttribute>();
				return attributes;
			}
		}
		
		public virtual string Documentation {
			get { return null; }
		}
		
		public Accessibility Accessibility {
			get { return accessibility; }
			set {
				CheckBeforeMutation();
				accessibility = value;
			}
		}
		
		public bool IsStatic {
			get { return flags[FlagStatic]; }
			set {
				CheckBeforeMutation();
				flags[FlagStatic] = value;
			}
		}
		
		public bool IsAbstract {
			get { return flags[FlagAbstract]; }
			set {
				CheckBeforeMutation();
				flags[FlagAbstract] = value;
			}
		}
		
		public bool IsSealed {
			get { return flags[FlagSealed]; }
			set {
				CheckBeforeMutation();
				flags[FlagSealed] = value;
			}
		}
		
		public bool IsShadowing {
			get { return flags[FlagShadowing]; }
			set {
				CheckBeforeMutation();
				flags[FlagShadowing] = value;
			}
		}
		
		public bool IsSynthetic {
			get { return flags[FlagSynthetic]; }
			set {
				CheckBeforeMutation();
				flags[FlagSynthetic] = value;
			}
		}
		
		public IProjectContent ProjectContent {
			get { return declaringTypeDefinition.ProjectContent; }
		}
		
		public string Name {
			get { return name; }
			set {
				CheckBeforeMutation();
				if (value == null)
					throw new ArgumentNullException();
				name = value;
			}
		}
		
		public virtual string FullName {
			get {
				return this.DeclaringType.FullName + "." + this.Name;
			}
		}
		
		public string Namespace {
			get { return declaringTypeDefinition.Namespace; }
		}
		
		public virtual string DotNetName {
			get { return this.DeclaringType.DotNetName + "." + this.Name; }
		}
	}
}

// Copyright 2015 ThoughtWorks, Inc.
//
// This file is part of Gauge-CSharp.
//
// Gauge-CSharp is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Gauge-CSharp is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with Gauge-CSharp.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using Gauge.CSharp.Runner.Models;

namespace Gauge.CSharp.Runner.Strategy
{
    [Serializable]
    public class TaggedHooksFirstStrategy : HooksStrategy
    {
        public override IEnumerable<string> GetApplicableHooks(IEnumerable<string> applicableTags,
            IEnumerable<IHookMethod> hooks)
        {
            var hookMethods = hooks as IList<IHookMethod> ?? hooks.ToList();
            var tags = applicableTags as IList<string> ?? applicableTags.ToList();
            return tags.Any()
                ? GetTaggedHooks(tags, hookMethods).Concat(GetUntaggedHooks(hookMethods))
                : GetUntaggedHooks(hookMethods);
        }
    }
}
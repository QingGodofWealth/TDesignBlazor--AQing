﻿using Microsoft.AspNetCore.Components.Rendering;

namespace TDesign;

/// <summary>
/// 表示时间轴的项。必须在 <see cref="TTimeline"/> 组件中使用。
/// </summary>
[ChildComponent(typeof(TTimeline))]
[CssClass("t-timeline-item")]
[HtmlTag("li")]
public class TTimelineItem : BlazorComponentBase, IHasChildContent
{
    /// <summary>
    /// <see cref="TTimeline"/> 父组件。
    /// </summary>
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
    [CascadingParameter] public TTimeline CascadingTimeline { get; set; }
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

    /// <inheritdoc/>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// 标签文本内容。
    /// </summary>
    [Parameter] public string? Label { get; set; }

    /// <summary>
    /// 设置节点的颜色。
    /// </summary>
    [Parameter] public Color Color { get; set; } = TDesign.Color.Primary;

    /// <summary>
    /// 自定义图标的名称。
    /// </summary>
    [Parameter] public object? IconName { get; set; }


    /// <inheritdoc/>
    protected override void BuildCssClass(ICssClassBuilder builder)
    {
        builder.Append($"t-timeline-item-{CascadingTimeline.LabelAlignment.ToString().ToLower()}");
    }

    /// <inheritdoc/>
    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.Div()
                .Class("t-timeline-item__wrapper")
                .Content(content =>
                {
                    content.Div()
                        .Class("t-timeline-item__dot", Color.GetCssClass("t-timeline-item__dot--"), (IconName is not null, "t-timeline-item__dot--custom"))
                        .Content(icon =>
                        {
                            icon.Div().Class("t-timeline-item__dot-content").Content(dot =>
                            {
                                if (IconName is not null)
                                {
                                    dot.Open<TIcon>().Attributes("Name", IconName).Close();
                                }
                            })
                            .Close();
                        })
                    .Close();

                    content.Div().Class("t-timeline-item__tail", CascadingTimeline.Theme.GetCssClass("t-timeline-item__tail--theme-"), "t-timeline-item__tail--status-primary")
                    .Close();
                })
        .Close();

        builder.Div()
                .Class("t-timeline-item__content")
                .Content(content =>
                {
                    content.AddContent(0, ChildContent);
                    if (!string.IsNullOrEmpty(Label))
                    {
                        content.Div().Class("t-timeline-item__label", $"t-timeline-item__label--{(CascadingTimeline.Alternate ? "alternate" : "same")}")
                                .Content(Label)
                        .Close();
                    }
                })
            .Close();
    }
}

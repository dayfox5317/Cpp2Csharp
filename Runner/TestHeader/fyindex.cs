//using Sichem;
//using System;
//using System.Runtime.InteropServices;

//namespace StbTrueTypeSharp
//{
//public	unsafe partial class StbTrueType
//	{
//		public const int STBRP_HEURISTIC_Skyline_default = 0;
//		public const int STBRP_HEURISTIC_Skyline_BL_sortHeight = STBRP_HEURISTIC_Skyline_default;
//		public const int STBRP_HEURISTIC_Skyline_BF_sortHeight = 2;
		
//		public const int STBRP__INIT_skyline = 1;
		
//		[StructLayout(LayoutKind.Sequential)]
//		public struct stbrp_rect
//	{
//		public int id;
//		public ushort w;
//		public ushort h;
//		public ushort x;
//		public ushort y;
//		public int was_packed;
//		}

//		[StructLayout(LayoutKind.Sequential)]
//		public struct stbrp_node
//	{
//		public ushort x;
//		public ushort y;
//		public stbrp_node* next;
//		}

//		[StructLayout(LayoutKind.Sequential)]
//		public struct stbrp_context
//	{
//		public int width;
//		public int height;
//		public int align;
//		public int init_mode;
//		public int heuristic;
//		public int num_nodes;
//		public stbrp_node* active_head;
//		public stbrp_node* free_head;
//		public stbrp_node* extra;
//		}

//		[StructLayout(LayoutKind.Sequential)]
//		public struct stbrp__findresult
//	{
//		public int x;
//		public int y;
//		public stbrp_node** prev_link;
//		}

//		public static void stbrp_setup_heuristic(stbrp_context* context, int heuristic)
//		{
//			switch (context->init_mode){
//case STBRP__INIT_skyline:context->heuristic = (int)(heuristic);break;default:break;
//            }

//		}

//		public static void stbrp_setup_allow_out_of_mem(stbrp_context* context, int allow_out_of_mem)
//		{
//			if ((allow_out_of_mem) != 0) context->align = (int)(1); else {
//context->align = (int)((context->width + context->num_nodes - 1) / context->num_nodes);}

//		}

//		public static void stbrp_init_target(stbrp_context* context, int width, int height, stbrp_node* nodes, int num_nodes)
//		{
//			int i = 0;
//		//	(void)(((width <= 0xffff) && (height <= 0xffff)) || ((_wassert("width <= 0xffff && height <= 0xffff", "TestHeader/imstb_rectpack.h", (uint)(268)) , 0) != 0));
//			for (i = (int)(0); (i) < (num_nodes - 1); ++i) {nodes[i].next = &nodes[i + 1];}
//			nodes[i].next = null;
//			context->init_mode = (int)(STBRP__INIT_skyline);
//			context->heuristic = (int)(STBRP_HEURISTIC_Skyline_default);
//			context->free_head = &nodes[0];
//			context->active_head = &context->extra[0];
//			context->width = (int)(width);
//			context->height = (int)(height);
//			context->num_nodes = (int)(num_nodes);
//			stbrp_setup_allow_out_of_mem(context, (int)(0));
//			context->extra[0].x = (ushort)(0);
//			context->extra[0].y = (ushort)(0);
//			context->extra[0].next = &context->extra[1];
//			context->extra[1].x = (ushort)(width);
//			context->extra[1].y = (ushort)(65535);
//			context->extra[1].next = null;
//		}

//		public static int stbrp__skyline_find_min_y(stbrp_context* c, stbrp_node* first, int x0, int width, int* pwaste)
//		{
//			stbrp_node* node = first;
//			int x1 = (int)(x0 + width);
//			int min_y = 0;int visited_width = 0;int waste_area = 0;
//			//(void)(c);
//			//(void)((first->x <= x0) || ((_wassert("first->x <= x0", "TestHeader/imstb_rectpack.h", (uint)(305)) , 0) != 0));
//			//(void)(((node->next->x) > (x0)) || ((_wassert("node->next->x > x0", "TestHeader/imstb_rectpack.h", (uint)(312)) , 0) != 0));
//			//(void)((node->x <= x0) || ((_wassert("node->x <= x0", "TestHeader/imstb_rectpack.h", (uint)(315)) , 0) != 0));
//			min_y = (int)(0);
//			waste_area = (int)(0);
//			visited_width = (int)(0);
//			while ((node->x) < (x1)) {
//if ((node->y) > (min_y)) {
//waste_area += (int)(visited_width * (node->y - min_y));min_y = (int)(node->y);if ((node->x) < (x0)) visited_width += (int)(node->next->x - x0); else visited_width += (int)(node->next->x - node->x);}
// else {
//int under_width = (int)(node->next->x - node->x);if ((under_width + visited_width) > (width)) under_width = (int)(width - visited_width);waste_area += (int)(under_width * (min_y - node->y));visited_width += (int)(under_width);}
//node = node->next;}
//			*pwaste = (int)(waste_area);
//			return (int)(min_y);
//		}

//		public static stbrp__findresult stbrp__skyline_find_best_pos(stbrp_context* c, int width, int height)
//		{
//			int best_waste = (int)(1 << 30);int best_x = 0;int best_y = (int)(1 << 30);
//			stbrp__findresult fr ;
//			stbrp_node** prev;stbrp_node* node;stbrp_node* tail;stbrp_node** best = null;
//			width = (int)(width + c->align - 1);
//			width -= (int)(width % c->align);
//			//(void)(((width % c->align) == (0)) || ((_wassert("width % c->align == 0", "TestHeader/imstb_rectpack.h", (uint)(362)) , 0) != 0));
//			node = c->active_head;
//			prev = &c->active_head;
//			while (node->x + width <= c->width) {
//int y = 0;int waste = 0;y = (int)(stbrp__skyline_find_min_y(c, node, (int)(node->x), (int)(width), &waste));if ((c->heuristic) == (STBRP_HEURISTIC_Skyline_BL_sortHeight)) {
//if ((y) < (best_y)) {
//best_y = (int)(y);best = prev;}
//}
// else {
//if (y + height <= c->height) {
//if (((y) < (best_y)) || (((y) == (best_y)) && ((waste) < (best_waste)))) {
//best_y = (int)(y);best_waste = (int)(waste);best = prev;}
//}
//}
//prev = &node->next;node = node->next;}
//			best_x = (int)(((best) == (null))?0:(*best)->x);
//			if ((c->heuristic) == (STBRP_HEURISTIC_Skyline_BF_sortHeight)) {
//tail = c->active_head;node = c->active_head;prev = &c->active_head;while ((tail->x) < (width)) {tail = tail->next;}while ((tail) != default) {
//int xpos = (int)(tail->x - width);int y = 0;int waste = 0;while (node->next->x <= xpos) {
//prev = &node->next;node = node->next;}y = (int)(stbrp__skyline_find_min_y(c, node, (int)(xpos), (int)(width), &waste));if ((y + height) < (c->height)) {
//if (y <= best_y) {
//if ((((y) < (best_y)) || ((waste) < (best_waste))) || (((waste) == (best_waste)) && ((xpos) < (best_x)))) {
//best_x = (int)(xpos);best_y = (int)(y);best_waste = (int)(waste);best = prev;}
//}
//}
//tail = tail->next;}}

//			fr.prev_link = best;
//			fr.x = (int)(best_x);
//			fr.y = (int)(best_y);
//			return (stbrp__findresult)(fr);
//		}

//		public static stbrp__findresult stbrp__skyline_pack_rectangle(stbrp_context* context, int width, int height)
//		{
//			stbrp__findresult res = (stbrp__findresult)(stbrp__skyline_find_best_pos(context, (int)(width), (int)(height)));
//			stbrp_node* node;stbrp_node* cur;
//			if ((((res.prev_link) == (null)) || ((res.y + height) > (context->height))) || ((context->free_head) == (null))) {
//res.prev_link = null;return (stbrp__findresult)(res);}

//			node = context->free_head;
//			node->x = (ushort)(res.x);
//			node->y = (ushort)(res.y + height);
//			context->free_head = node->next;
//			cur = *res.prev_link;
//			if ((cur->x) < (res.x)) {
//stbrp_node* next = cur->next;cur->next = node;cur = next;}
// else {
//*res.prev_link = node;}

//			while (((cur->next) != default) && (cur->next->x <= res.x + width)) {
//stbrp_node* next = cur->next;cur->next = context->free_head;context->free_head = cur;cur = next;}
//			node->next = cur;
//			if ((cur->x) < (res.x + width)) cur->x = (ushort)(res.x + width);
//			return (stbrp__findresult)(res);
//		}

//		public static int rect_height_compare(void * a, void * b)
//		{
//			stbrp_rect* p = (stbrp_rect*)(a);
//			stbrp_rect* q = (stbrp_rect*)(b);
//			if ((p->h) > (q->h)) return (int)(-1);
//			if ((p->h) < (q->h)) return (int)(1);
//            var gs = ((p->w) < (q->w))?1:0;

//            var bs=(((p->w) > (q->w))?-1:(gs));
//            return bs;
//        }

//		public static int rect_original_order(void * a, void * b)
//		{
//			stbrp_rect* p = (stbrp_rect*)(a);
//			stbrp_rect* q = (stbrp_rect*)(b);
//			return (int)(((p->was_packed) < (q->was_packed))?-1:(((p->was_packed) > (q->was_packed))?1:0));
//		}

//		public static int stbrp_pack_rects(stbrp_context* context, stbrp_rect* rects, int num_rects)
//		{
//			int i = 0;int all_rects_packed = (int)(1);
//			for (i = (int)(0); (i) < (num_rects); ++i) {
//rects[i].was_packed = (int)(i);}
//			CRuntime.qsort(rects, (ulong)(num_rects), (ulong)(sizeof(stbrp_rect)), rect_height_compare);
//			for (i = (int)(0); (i) < (num_rects); ++i) {
//if (((rects[i].w) == (0)) || ((rects[i].h) == (0))) {
//rects[i].x = (ushort)(rects[i].y = (ushort)(0));}
// else {
//stbrp__findresult fr = (stbrp__findresult)(stbrp__skyline_pack_rectangle(context, (int)(rects[i].w), (int)(rects[i].h)));if ((fr.prev_link) != default) {
//rects[i].x = (ushort)(fr.x);rects[i].y = (ushort)(fr.y);}
// else {
//rects[i].x = (ushort)(rects[i].y = (ushort)(0xffff));}
//}
//}
//			CRuntime.qsort(rects, (ulong)(num_rects), (ulong)(sizeof(stbrp_rect)), rect_original_order);
//			for (i = (int)(0); (i) < (num_rects); ++i) {
//rects[i].was_packed = (!(((rects[i].x) == (0xffff)) && ((rects[i].y) == (0xffff))))?1:0;if (rects[i].was_packed== 0) all_rects_packed = (int)(0);}
//			return (int)(all_rects_packed);
//		}

//	}
//}
